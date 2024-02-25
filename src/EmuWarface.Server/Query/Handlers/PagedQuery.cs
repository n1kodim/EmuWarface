using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Xmpp;
using NLog;
using XmppDotNet.Xmpp.Client;
using XmppDotNet.Xmpp.MessageArchiving;

namespace EmuWarface.Server.Query.Handlers
{
    #region Xml samples

    /*
    Request:   masterserver@warface/xxx
    <items/>
    <items from='250' hash='1487642888'/>


    Response:   ms.warface

    2 - HasMore
    <data query_name='items' code='2' from='250' to='500' hash='1487642888' compressedData='xxx' originalSize='xxx'/>
    3 - Done
    <data query_name='items' code='3' from='4250' to='4383' hash='1487642888' compressedData='xxx' originalSize='xxx'/>
    */

    #endregion

    [Query("items")]
    public class PagedQuery : IQueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            var from = query.Element.GetAttributeInt("from");
			var to = query.Element.HasAttribute("to") ? query.Element.GetAttributeInt("to") : int.MaxValue;
			var requestHash = query.Element.GetAttribute("hash");
			var cachedHash = query.Element.GetAttribute("cached");

            if (from < 0 || to <= from)
                return -1;


            return 0;
        }

        public int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            throw new NotImplementedException();
        }

        private enum ResCode
        {
            RequestSequenceInterrupted,
            NotModified,
            HasMore,
            Done
        }
    }
}
