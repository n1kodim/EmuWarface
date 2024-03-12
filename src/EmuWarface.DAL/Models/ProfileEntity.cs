namespace EmuWarface.DAL.Models;

public class ProfileEntity
{
    public ulong Id { get; set; }
    public string Nickname { get; set; }
    public string Head { get; set; }
    public ulong Experience { get; set; }
    public byte CurrentClass { get; set; }
    public byte MissionPassed { get; set; }
    public uint BannerBadge { get; set; } = uint.MaxValue;
    public uint BannerMark { get; set; } = uint.MaxValue;
    public uint BannerStripe { get; set; } = uint.MaxValue;
    public ulong GameMoney { get; set; }
    public ulong CrownMoney { get; set; }
    public ulong CryMoney { get; set; }
    public DateTime LastSeen { get; set; } = DateTime.Now;
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public DateTime? RatingBanExpire { get; set; }
    public DateTime? BanExpire { get; set; }
    public DateTime? MuteExpire { get; set; }

    public ulong UserId { get; set; }
    public UserEntity User { get; set; }
    public ClanMemberEntity? ClanMember { get; set; }
    public List<ClanKickEntity> ClanKicks { get; set; } = new List<ClanKickEntity>();
    public List<ProfileAchievementEntity> Achievements { get; set; } = new List<ProfileAchievementEntity>();
    public List<ProfileItemEntity> Items { get; set; } = new List<ProfileItemEntity>();
    public List<FriendEntity> Friends { get; set; } = new List<FriendEntity>();

}
