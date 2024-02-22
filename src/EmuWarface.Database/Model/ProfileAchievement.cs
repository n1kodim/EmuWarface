using System;
using System.Collections.Generic;

namespace EmuWarface.Database.Model;

public partial class ProfileAchievement
{
    public int AchievementId { get; set; }

    public ulong ProfileId { get; set; }

    public int Progress { get; set; }

    public DateTime CompletionTime { get; set; }

    public virtual Profile Profile { get; set; } = null!;
}
