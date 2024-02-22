using NLog;
using System.Net.Sockets;
using System.Text;
using XmppDotNet;
using XmppDotNet.Xml;
using XmppDotNet.Xmpp;
using XmppDotNet.Xmpp.Bind;
using XmppDotNet.Xmpp.Sasl;
using XmppDotNet.Xmpp.Session;
using XmppDotNet.Xmpp.Stream;
using XmppDotNet.Xmpp.Tls;
using XmppDotNet.Xmpp.Client;
using EmuWarface.Server.Network;
using EmuWarface.Server.CryOnline.Xmpp.Server;
using EmuWarface.Server.CryOnline.Xmpp;
using EmuWarface.Server.Query;
using EmuWarface.Server.Game.Player;

namespace EmuWarface.Server.CryOnline
{
    public class XmppSession : NetworkSession
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Jid Jid { get; private set; }
        public SessionState State { get; private set; }
        public string User { get; private set; }
        public string Resource { get; private set; }
        public PlayerStatus Status { get; private set; }

        public event Action<XmppSession, XmppXElement> OnXmlReceived;
        public event Action<XmppSession, XmppXElement> OnXmlSent;
        public event Action<XmppSession, Iq, CryOnlineQuery> OnQuery;
        public event Action<XmppSession> OnBinded;
        public event Action<XmppSession> OnDisconnect;

        public XmppSession()
            : base()
        {
            _streamParser.OnStreamStart += OnStreamStart;
            _streamParser.OnStreamElement += OnStreamElement;
            _streamParser.OnStreamEnd += OnStreamEnd;
            _streamParser.OnStreamError += OnStreamError;

            State = SessionState.Connected;
        }

        protected override void OnDisconnected()
        {
            OnDisconnect?.Invoke(this);
        }

        public XmppXElement GetStreamHeader(Jid from, string version) => new XmppDotNet.Xmpp.Server.Stream { From = from, Version = version, IsStartTag = true };
        public XmppXElement GetStreamFooter() => new XmppDotNet.Xmpp.Server.Stream { IsEndTag = true };
        private StreamFeatures GetStreamFeatures()
        {
            StreamFeatures feats = new StreamFeatures();

            switch (State)
            {
                case SessionState.Connected:
                    State = SessionState.Securing;
                    feats.StartTls = new StartTls();
                    break;
                case SessionState.Secure:
                    State = SessionState.Authenticating;
                    feats.Mechanisms = new Mechanisms();
                    // TODO: Patch XmppDotNet enum, to add Warface
                    feats.Mechanisms.AddMechanism(SaslMechanism.Plain);
                    break;
                case SessionState.Authenticated:
                    State = SessionState.Binding;
                    feats.Bind = new Bind();
                    feats.Session = new Session();
                    break;
                default:
                    // TODO: ERROR
                    break;
            }

            return feats;
        }

        private async void OnStreamStart(XmppXElement el)
        {
            el.IsStartTag = true;

            OnXmlReceived?.Invoke(this, el);

            await SendAsync(GetStreamHeader(Globals.XmppDomain, "1.0"));
            await SendAsync(GetStreamFeatures());
        }

        private void OnStreamElement(XmppXElement el)
        {
            OnXmlReceived?.Invoke(this, el);

            if (el is StartTls starttls)
            {
                ProcessStartTls(starttls);
            }
            else if (el is Auth auth)
            {
                ProcessSaslPlainAuth(auth);
            }
            else if (el is Iq iq)
            {
                ProcessIq(iq);
            }
        }

        private async void OnStreamEnd()
        {
            await SendAsync(GetStreamFooter());
        }

        private async void OnStreamError(Exception e)
        {
            _logger.Error(e);
            await DisconnectAsync();
        }

        private async void ProcessStartTls(StartTls el)
        {
            if (State != SessionState.Securing)
            {
                await DisconnectAsync();
                return;
            }

            _cts.Cancel();

            await SendAsync(new Proceed());
            if (!await HandshakeAsync())
            {
                _logger.Warn("{0} failed to secure TLS", RemoteAddress);
                // TODO: Log error
                return;
            }
            _cts = new CancellationTokenSource();
            _streamParser.Reset();

            State = SessionState.Secure;
            StartReceiverTask();
        }

        private async void ProcessSaslPlainAuth(Auth el)
        {
            if (State != SessionState.Authenticating ||
                el.SaslMechanism != SaslMechanism.Plain ||
                string.IsNullOrEmpty(el.Value))

            {
                // TODO
                _logger.Warn(@$"Client {RemoteAddress} tried to auth not by 'WARFACE'");
                await DisconnectAsync();
                return;
            }

            /*if (!appsett.XmppServer.AllowAnyone)
            {
                ReadOnlySpan<string> data = Encoding.UTF8.GetString(el.Bytes).Split('\x000', StringSplitOptions.RemoveEmptyEntries);

            // TODO: Check in DB
            }*/
            string[] data = Encoding.UTF8.GetString(el.Bytes).Split('\x000', StringSplitOptions.RemoveEmptyEntries);
            User = data[0];

            _streamParser.Reset();

            State = SessionState.Authenticated;
            await SendAsync(new Success());
        }

        private void ProcessIq(Iq iq)
        {
            //if (iq.Query is Roster)
            //ProcessRosterIq(iq);
            /*else*/
            if (iq.Query is Bind bind)
                ProcessBind(iq, bind);
            else if (iq.Query is Session session)
                ProcessSession(iq, session);
            else if (iq.Query is CryOnlineQuery query)
                OnQuery(this, iq, query);
        }

        private async void ProcessBind(Iq iq, Bind bind)
        {
            // TODO: Dedicated allowed with config IPs
            //if(bind.Resource != "GameClient")

            string resource = bind.Resource;
            if (string.IsNullOrEmpty(resource))
            {
                // TODO: Kick
                return;
            }

            var jid = new Jid(User, Globals.XmppDomain, resource);
            Jid = jid;
            var resIq = new XmppDotNet.Xmpp.Client.BindIq
            {
                Id = iq.Id,
                Type = IqType.Result,
                Bind = { Jid = jid }
            };

            await SendAsync(resIq);
            Resource = resource;
            State = SessionState.Binded;
            OnBinded?.Invoke(this);
        }

        private async void ProcessSession(Iq iq, Session session)
        {
            iq.SwitchDirection();
            iq.Type = IqType.Result;

            await SendAsync(iq);
        }

        public async Task<bool> SendAsync(XmppXElement el)
        {
            var str = el.ToString(false);

            // Needed patch element for GameClient
            str = str.Replace("xmlns=\"jabber:server\"", "");

            var data = Encoding.UTF8.GetBytes(str);

            if (await SendAsync(data) != 0)
            {
                OnXmlSent?.Invoke(this, el);
                return true;
            }
            return false;
        }

        public async void SendQueryResponse(Iq iq, XmppXElement? query = null, Jid? sender = null, int customErrorCode = 0)
        {
            SendQueryResponse(iq, new CryOnlineQuery(query), sender, customErrorCode);
        }

        public async void SendQueryResponse(Iq iq, CryOnlineQuery? query = null, Jid? sender = null, int customErrorCode = 0)
        {
            sender = sender != null ? sender : iq.To;

            QueryIq response = new QueryIq(Jid, sender, IqType.Result, iq.Id)
            {
                Query = query,
                Error = customErrorCode != 0 ? new QueryError(customErrorCode) : null
            };

            await SendAsync(response);
        }

        protected override void OnError(SocketError error)
        {
            // TODO: Handle error
            _logger.Warn("{0}:{1} disconnected with error: {2}", RemoteAddress, RemotePort, error.GetName());
        }
    }
}