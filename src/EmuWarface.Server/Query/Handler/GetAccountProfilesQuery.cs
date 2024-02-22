using EmuWarface.Database;
using EmuWarface.Database.Model;
using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Xmpp;
using EmuWarface.Server.Data;
using NLog;
using XmppDotNet.Xml;
using XmppDotNet.Xmpp.Client;

namespace EmuWarface.Server.Query.Handler
{
    #region Xml samples

    /*
    Request:   masterserver@warface/xxx
    <get_account_profiles version="1.15000.124.34300" user_id="xxx" token=" " />

    Response:  masterserver@warface/xxx
    <profile id='xxx' nickname='xxx'/>
    */

    #endregion

    [Query("get_account_profiles")]
    public class GetAccountProfilesQuery : IQueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private IGameDatabase _gameDatabase;
        private GameResources _resourceManager;

        public GetAccountProfilesQuery(GameResources resourceManager, IGameDatabase gameDatabase)
        { 
            _resourceManager = resourceManager;
            _gameDatabase = gameDatabase;
        }

        public int HandleRequest(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            string version = query.Element.GetAttribute("version");

            if (version != _resourceManager.GameConfigSettings.GameVersion)
            {
                _logger.Warn("\"{0}\" trying to log in with the wrong version of the game ({1})", session.Jid, version);
                return 1;
            }

            uint user_id = (uint)query.Element.GetAttributeInt("user_id");
            if(user_id.ToString() != session.User)
            {
                _logger.Warn("\"{0}\" trying to log in with invalid \"user_id\" ({1})", session.Jid, user_id);
                return 2;
            }

            Profile? profile = _gameDatabase
                .GetProfileByUserIdAsync(user_id)
                .GetAwaiter()
                .GetResult();

            var res = new XmppXElement(query.Element.Name);

            if (profile != null)
            {
                var profileEl = new XmppXElement(Namespaces.CryOnline, "profile");
                profileEl.SetAttribute("id", profile.Id);
                profileEl.SetAttribute("nickname", profile.Nickname);

                res.Add(profileEl);
            }

            session.SendQueryResponse(iq, res);
            return 0;
        }

        public int HandleResponse(XmppSession session, Iq iq, CryOnlineQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
