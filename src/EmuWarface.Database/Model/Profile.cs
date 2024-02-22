using System;
using System.Collections.Generic;

namespace EmuWarface.Database.Model;

public partial class Profile
{
    public ulong Id { get; set; }

    public ulong UserId { get; set; }

    public string Nickname { get; set; } = null!;

    public float Height { get; set; }

    public float Fatness { get; set; }

    public string Head { get; set; } = null!;

    public uint? CurrentClass { get; set; }

    public ulong Experience { get; set; }

    public byte MissionPassed { get; set; }

    public uint BannerBadge { get; set; }

    public uint BannerMark { get; set; }

    public uint BannerStripe { get; set; }

    public DateTime LastSeen { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime BanExpire { get; set; }

    public DateTime MuteExpire { get; set; }

    public virtual ICollection<ProfileAchievement> ProfileAchievements { get; } = new List<ProfileAchievement>();

    public virtual ICollection<ProfileItem> ProfileItems { get; } = new List<ProfileItem>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
