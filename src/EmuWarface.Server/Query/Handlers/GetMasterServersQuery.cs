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
    public class GetMasterServersQuery : IQueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private ChannelManager _channelManager;

        public GetMasterServersQuery(ChannelManager channelManager)
        {
            _channelManager = channelManager;
        }

        public int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            var res = new XmppXElement(query.Element.Name);

            if (query.Element.Name.LocalName == "account")
            {
                // TODO: it can be validated
                string login = query.Element.GetAttribute("login");
                string password = query.Element.GetAttribute("password");

                res.SetAttribute("user", iq.From.Local)
                    .SetAttribute("survival_lb_enabled", "0")
                    .SetAttribute("active_token", " ")
                    .SetAttribute("nickname", "");
            }

            var ms = new XmppXElement(Namespaces.CryOnline, "masterservers");
            foreach(var server in _channelManager.GetChannels())
            {
                ms.Add(server.Serialize());
            }
            res.Add(ms);

            session.SendQueryResponse(iq, res);
            return 0;
        }

        public int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
