using System.Runtime.Serialization;
using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Xmpp;
using XmppDotNet.Xmpp.Client;

namespace EmuWarface.Server.Query
{
    public interface IQueryHandler
    {
        int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query);
        int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query);
    }
}
