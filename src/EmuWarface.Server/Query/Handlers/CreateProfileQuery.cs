using EmuWarface.DAL.Models;
using EmuWarface.DAL.Repositories;
using EmuWarface.Server.Common;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Common.Configuration;
using EmuWarface.Server.Game;
using EmuWarface.Server.Game.Player;
using EmuWarface.Server.Game.Profile;
using MiniXML;
using NLog;
using System;

namespace EmuWarface.Server.Query.Handlers
{
    #region Xml samples

    /*
    Request:   k01.warface
    <create_profile user_id='xxx' version='1.xxx.xxx.xxx' token=' ' 
    nickname='xxx' region_id='global' head='default_head_xx' resource='pvp_newbie_xxx' 
    hw_id='670721246' cpu_vendor='1' cpu_family='6' cpu_model='10' cpu_stepping='7' cpu_speed='3391' cpu_num_cores='4' 
    gpu_vendor_id='4318' gpu_device_id='2121' physical_memory='4079' os_ver='5' os_64='1' build_type='--release'/>

    Response:  masterserver@warface/xxx
    <create_profile profile_id='xxx'>
        <character nick='xxx' gender='male' height='1' fatness='0' head='default_head_xx'
            current_class='0' experience='0' pvp_rating_rank='0' pvp_rating_games_history=''
            banner_badge='4294967295' banner_mark='4294967295' banner_stripe='4294967295'
            game_money='0' cry_money='0' crown_money='0'>
            <ProfileBans />
            <item id='xxx' name='rds07' attached_to='0' config='dm=0;material=default'
                slot='21647380' equipped='29' default='1' permanent='0' expired_confirmed='0'
                buy_time_utc='0' expiration_time_utc='0' seconds_left='0' />
            ...
            <sponsor_info>
                <sponsor sponsor_id='0' sponsor_points='0' next_unlock_item='smg12_shop' />
                <sponsor sponsor_id='1' sponsor_points='0' next_unlock_item='medic_vest_02' />
                <sponsor sponsor_id='2' sponsor_points='0' next_unlock_item='gp03' />
            </sponsor_info>
            <chat_channels>
                <chat channel='0' channel_id='global.pvp_newbie_xxx' service_id='conference.warface' />
            </chat_channels>
            <profile_progression_state profile_id='xxx' mission_unlocked='none,trainingmission,all'
                tutorial_unlocked='1' tutorial_passed='0' class_unlocked='5' />
            <login_bonus current_streak='0' current_reward='-1' />
            <variables>
                <item key='max_xp_boost_effect' value='2.95' />
                ...
            </variables>
        </character>
    </create_profile>
    */

    #endregion

    [Query("create_profile")]
    public class CreateProfileQuery : QueryHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly GameOptions _gameOptions;
        private readonly ProfileManager _profileManager;
        private readonly ProfileRepository _profileRepository;
        private readonly InputStringValidator _inputStringValidator;

        public CreateProfileQuery(GameOptions gameOptions, ProfileManager profileManager, ProfileRepository profileRepository, InputStringValidator inputStringValidator)
        {
            _gameOptions = gameOptions;
            _profileManager = profileManager;
            _profileRepository = profileRepository;
            _inputStringValidator = inputStringValidator;
        }

        public override async Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            string regionId = query.GetAttribute("region_id");
            ulong userId = query.GetAttributeValue<ulong>("user_id");
            string head = query.GetAttribute("head");
            string token = query.GetAttribute("token");
            string nickname = query.GetAttribute("nickname");
            string buildType = query.GetAttribute("build_type");
            string gameVersion = query.GetAttribute("version");
            string hardwareId = query.GetAttribute("hw_id");
            uint cpuVendor = query.GetAttributeValue<uint>("cpu_vendor");
            uint cpuFamily = query.GetAttributeValue<uint>("cpu_family");
            uint cpuModel = query.GetAttributeValue<uint>("cpu_model");
            uint cpuStepping = query.GetAttributeValue<uint>("cpu_stepping");
            uint cpuSpeed = query.GetAttributeValue<uint>("cpu_speed");
            uint cpuNumCores = query.GetAttributeValue<uint>("cpu_num_cores");
            uint gpuVendorId = query.GetAttributeValue<uint>("gpu_vendor_id");
            uint gpuDeviceId = query.GetAttributeValue<uint>("gpu_device_id");
            uint physicalMemory = query.GetAttributeValue<uint>("physical_memory");
            uint os64 = query.GetAttributeValue<uint>("os_64");
            uint osVer = query.GetAttributeValue<uint>("os_ver");

            if (userId.ToString() != player.Jid.Local)
            {
                _logger.Warn("Wrong user_id attribute: {0} ({1})", userId, player.Jid);
                return (int)ErrorCode.InternalError;
            }

            if (gameVersion != _gameOptions.Version || buildType != "--release")
            {
                _logger.Warn("Wrong game version when creating a profile: version={0} ({1})", gameVersion, player.Jid);
                return (int)ErrorCode.GameVersionMismatch;
            }

            if (!_inputStringValidator.ValidateNickname(nickname, _gameOptions.Language))
            {
                _logger.Warn("Invalid nickname when creating a profile: nickname={0} ({1})", nickname, player.Jid);
                return (int)ErrorCode.InvalidNickname;
            }

            if(!_profileManager.ValidateHead(head))
            {
                _logger.Warn("Specified head is invalid or not found on the server items: head={0} ({1})", head, player.Jid);
                return (int)ErrorCode.InvalidHead;
            }

            ProfileEntity? profile = await _profileRepository.GetByUserIdAsync(userId);

            if (profile != null)
            {
                _logger.Warn("The user already has a profile: pid={0} ({1})", profile.Id, player.Jid);
                return (int)ErrorCode.AllreadyExist;
            }

            profile = await _profileRepository.Add(userId, nickname, head);

            if (profile == null)
            {
                _logger.Warn("Specified nickname is already used by another player: nickname={0} ({1})", nickname, player.Jid);
                return (int)ErrorCode.InternalNicknameCollision;
            }




            return _profileManager.SerializeCharacter(profile);
            //return 0;
        }
    }

    file enum ErrorCode
    {
        InternalError = -1,
        None,
        AllreadyExist = 1,
        InvalidNickname = 2,
        ReservedNickname = 3,
        GameVersionMismatch = 4,
        InternalNicknameCollision= 7,
        InvalidHead = 8
    }
}
