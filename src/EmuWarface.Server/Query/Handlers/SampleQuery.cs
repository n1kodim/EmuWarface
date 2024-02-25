using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Xmpp;
using NLog;
using XmppDotNet.Xmpp.Client;

namespace EmuWarface.Server.Query.Handlers
{
    #region Xml samples

    /*
    Request:   k01.warface

    Response:   k01.warface

    */

    #endregion

    [Query("query_name")]
    public class NameQuery : IQueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            return 0;
        }

        public int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
