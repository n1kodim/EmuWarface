using XmppDotNet.Xmpp;
using XmppDotNet;
using XmppDotNet.Xmpp.Server;

namespace EmuWarface.Server.CryOnline.Xmpp.Server
{
    public class QueryIq : Iq
    {
        public QueryIq()
            : base()
        {
            Add(new CryOnlineQuery());
            Id = Xmpp.Id.GetNextId();
        }

        public QueryIq(IqType type)
            : this()
        {
            Type = type;
        }

        public QueryIq(Jid to)
            : this()
        {
            To = to;
        }

        public QueryIq(Jid to, Jid from)
            : this(to)
        {
            From = from;
        }

        public QueryIq(Jid to, Jid from, IqType type)
            : this(to, from)
        {
            Type = type;
        }

        public QueryIq(Jid to, Jid from, IqType type, string id)
            : this(to, from, type)
        {
            Id = id;
        }

        public CryOnlineQuery Query
        {
            get { return Element<CryOnlineQuery>(); }
            set { Replace(value); }
        }
    }
}
