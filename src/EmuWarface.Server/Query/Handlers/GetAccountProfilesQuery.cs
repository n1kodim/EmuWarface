using EmuWarface.DAL.Models;
using EmuWarface.DAL.Repositories;
using EmuWarface.Server.Common;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Common.Configuration;
using EmuWarface.Server.Game.Data;
using EmuWarface.Server.Game.Player;
using Microsoft.Extensions.Configuration;
using MiniXML;
using NLog;

namespace EmuWarface.Server.Query.Handlers
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
    public class GetAccountProfilesQuery : QueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly GameOptions _gameOptions;
        private readonly ProfileRepository _profileRepository;

        public GetAccountProfilesQuery(IConfiguration configuration, GameOptions gameOptions, ProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _gameOptions = gameOptions;
        }

        public override async Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            string version = query.GetAttribute("version");
            uint user_id = query.GetAttributeValue<uint>("user_id");

            if (version != _gameOptions.Version)
            {
                _logger.Warn("[{0}] Wrong 'version': {1}", player.Jid, version);
                return (int)ErrorCode.WrongGameVersion;
            }
            if(user_id.ToString() != player.Jid.Local)
            {
                _logger.Warn("[{0}] Wrong 'user_id': {1}", player.Jid, user_id);
                return (int)ErrorCode.WrongUserId;
            }

            ProfileEntity? profile = await _profileRepository.GetByUserIdAsync(user_id);

            Element res = new Element(query.Name);

            if (profile != null)
            {
                var profileEl = new Element("profile");
                profileEl.SetAttributeValue("id", profile.Id);
                profileEl.SetAttribute("nickname", profile.Nickname);

                res.C(profileEl);
            }

            return res;
        }
    }

    file enum ErrorCode
    {
        None,
        WrongGameVersion,
        WrongUserId
    }
}
