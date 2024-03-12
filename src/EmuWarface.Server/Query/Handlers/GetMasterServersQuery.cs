using EmuWarface.Server.Common;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Game.Channel;
using EmuWarface.Server.Game.Player;
using MiniXML;
using NLog;

namespace EmuWarface.Server.Query.Handler
{
    #region Xml samples

    /*
    Request:   k01.warface

    <get_master_servers/>
    <account login='xxx' password='xxx'/>
    

    Response:   k01.warface

    <account user='xxx' active_token=' ' nickname='' survival_lb_enabled='0'>
        <masterservers>
            ...
        </masterservers>
    </account>

    <get_master_servers>
        <masterservers>
            <server resource='pvp_pro_69_r1' server_id='369'
            channel='pvp_pro' rank_group='all' load='0.982143'
            online='1122' min_rank='26' max_rank='90' bootstrap=''>
                <load_stats>
                <load_stat type='quick_play' value='255'/>
                <load_stat type='survival' value='255'/>
                <load_stat type='pve' value='255'/>
                </load_stats>
            </server>
            ...
        </masterservers>
    </get_master_servers>
    */


    #endregion

    [Query("account", "get_master_servers")]
    public class GetMasterServersQuery : QueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private ChannelManager _channelManager;

        public GetMasterServersQuery(ChannelManager channelManager)
        {
            _channelManager = channelManager;
        }

        public override async Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            var res = new Element(query.Name);

            if (query.LocalName == "account")
            {
                // TODO: it can be validated
                string login = query.GetAttribute("login");
                string password = query.GetAttribute("password");

                res.SetAttribute("user", player.Jid.Local);
                res.SetAttribute("survival_lb_enabled", "0");
                res.SetAttribute("active_token", " ");
                res.SetAttribute("nickname", "");
            }

            var ms = new Element("masterservers");
            foreach(var server in _channelManager.GetChannels())
            {
                ms.C(server.Serialize());
            }
            res.C(ms);

            return res;
        }
    }
}
