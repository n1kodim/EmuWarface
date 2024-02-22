using EmuWarface.Common.Enums;
using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Xmpp;
using EmuWarface.Server.Game.Channel;
using NLog;
using XmppDotNet.Xml;
using XmppDotNet.Xmpp.Client;

namespace EmuWarface.Server.Query.Handler
{
    #region Xml samples

    /*
    Request:   k01.warface
    <get_master_server rank='x' channel='pve'/>
    <get_master_server rank='x' channel='pve' search_type='pve'/>
    <get_master_server rank='4' channel='pve' used_resources='pve_001;pve_002;...'/>

    Response:   k01.warface
    <get_master_server resource='pve_001' load_index='255'/>
    */

    #endregion

    [Query("get_master_server")]
    public class GetMasterServerQuery : IQueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private ChannelManager _channelManager;

        public GetMasterServerQuery(ChannelManager channelManager)
        {
            _channelManager = channelManager;
        }

        public int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            ChannelType channel = query.Element.GetAttributeEnum<ChannelType>("channel");
            if ((int)channel == -1)
            {
                _logger.Error("Unknown channnel type: \"{0}\"", channel);
                return 2;
            }

            MasterServer? ms = _channelManager.GetChannel(channel);

            // TODO: Validate rank and used_resources

            //int rank = query.Element.GetAttributeInt("rank");
            //IEnumerable<string> usedResources = query.Element.GetAttribute("used_resources").Split(';');

            var el = new XmppXElement(query.Element.Name);
            el.SetAttribute("resource", ms.Resource);
            el.SetAttribute("load_index", "255");

            session.SendQueryResponse(iq, el);

            return 0;
        }

        public int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
