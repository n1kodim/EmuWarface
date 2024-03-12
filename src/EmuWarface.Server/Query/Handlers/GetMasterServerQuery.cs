using EmuWarface.Common;
using EmuWarface.Server.Common;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Game.Channel;
using EmuWarface.Server.Game.Player;
using MiniXML;
using NLog;

namespace EmuWarface.Server.Query.Handlers
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
    public class GetMasterServerQuery : QueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private ChannelManager _channelManager;

        public GetMasterServerQuery(ChannelManager channelManager)
        {
            _channelManager = channelManager;
        }

        public override async Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            var channel = query.GetAttribute("channel").ToEnum<ChannelType>();
            /*if ((int)channel == -1)
            {
                _logger.Error("Unknown channnel type: \"{0}\"", channel);
                return 2;
            }*/
            // TODO: check channel type

            MasterServer? ms = _channelManager.GetChannel(channel);

            // TODO: Validate rank and used_resources

            //int rank = query.Element.GetAttributeInt("rank");
            //IEnumerable<string> usedResources = query.Element.GetAttribute("used_resources").Split(';');

            Element res = new Element(query.Name);
            res.SetAttribute("resource", ms.Resource);
            res.SetAttribute("load_index", "255");

            return res;
        }
    }
}
