using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using EmuWarface.Common;
using EmuWarface.Common.Configuration;
using EmuWarface.Server.CryOnline.Xmpp;
using EmuWarface.Server.Network;
using EmuWarface.Server.Query;
using NLog;
using System.Net.Sockets;
using XmppDotNet;
using XmppDotNet.Xml;
using XmppDotNet.Xmpp;
using XmppDotNet.Xmpp.Client;

namespace EmuWarface.Server.CryOnline
{
    public class XmppServer : SocketListener<XmppSession>
    {
        private readonly Logger _logger = LogManager.GetLogger("XMPP");

        private QueryFactory _queryFactory;
        private AppSettings _appSettings;

        public event Action<XmppSession> OnNewSession;

        public XmppServer(QueryFactory queryFactory, AppSettings appSettings)
        {
            _queryFactory = queryFactory;
            _appSettings = appSettings;
        }

        public bool Start()
        {
            var host = _appSettings.XmppServer.Host;
            var port = _appSettings.XmppServer.Port;

            _logger.Info("Starting server...");

            if (Start(host, port))
            {
                _logger.Info("Server started on {0}:{1}.", host, port);
                return true;
            }

            return false;
        }

        public void Stop()
        {

        }

        public XmppSession? GetSession(Jid jid)
        {
            XmppSession? res;
            lock (_lock)
            {
                res = _sessions.SingleOrDefault(x => x.Jid.CompareTo(jid) == 0);
            }
            return res;
        }

        public IEnumerable<XmppSession> GetSessions(Jid jid)
        {
            IEnumerable<XmppSession> res;
            lock (_lock)
            {
                res = _sessions.Where(x => x.Jid.CompareTo(jid) == 0).ToList();
            }
            return res;
        }

        public void Kick(XmppSession target)
        {
            throw new NotImplementedException();
        }

        public void Kick(Jid jid)
        {
            throw new NotImplementedException();
        }

        public void KickAll()
        {
            throw new NotImplementedException();
        }

        protected override void OnError(SocketError error)
        {
            // TODO: Handle error
        }

        protected override void OnSessionConnected(XmppSession session)
        {
            session.OnXmlReceived += OnSessionXmlReceived;
            session.OnXmlSent += OnSessionXmlSent;
            session.OnBinded += OnSessionBinded;
            session.OnQuery += OnSessionQuery;
            session.OnDisconnect += OnSessionDisconnect;

            _logger.Info("{0}:{1} connecting to the server", session.RemoteAddress, session.RemotePort);
        }

        private void OnSessionDisconnect(XmppSession session)
        {
            lock (_lock)
            {
                _sessions.Remove(session);
            }

            _logger.Info("{0}:{1} disconnected from the server", session.RemoteAddress, session.RemotePort);
        }

        private void OnSessionBinded(XmppSession session)
        {
            foreach (XmppSession target in GetSessions(session.Jid))
            {
                if (session != target)
                {
                    _logger.Warn("{0}:{1} {2} existing on the server (kick)", target.RemoteAddress, target.RemotePort, target.Jid);
                    target.DisconnectAsync().GetAwaiter().GetResult();
                }
            }

            _logger.Info("{0}:{1} binded with \"{2}\"", session.RemoteAddress, session.RemotePort, session.Jid);
        }


        private void OnSessionQuery(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            int errorCode = 0;

            IQueryHandler? handler = _queryFactory.GetHandler(query);
            if (handler == null)
            {
                _logger.Warn($"Unhandled query \"{query.Element?.Name?.LocalName}\" from: {session.Jid}");
                session.SendQueryResponse(iq, query, customErrorCode: -1);
                return;
            }

            try
            {
                errorCode = iq.Type == IqType.Get
                    ? handler.HandleRequest(session, iq, query)
                    : handler.HandleResponse(session, iq, query);
            }
            catch
            {
                throw;
            }

            if (errorCode != 0)
                session.SendQueryResponse(iq, query, customErrorCode: errorCode);
        }

        private void OnSessionXmlSent(XmppSession session, XmppXElement elem)
        {
            _logger.Xmpp(elem.ToString(true), session.RemoteAddress, false);
        }

        private void OnSessionXmlReceived(XmppSession session, XmppXElement elem)
        {
            _logger.Xmpp(elem.ToString(true), session.RemoteAddress, true);
        }
    }
}
