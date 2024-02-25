namespace EmuWarface.DAL.Models;

public class ProfileAchievementEntity
{
    public int Id { get; set; }
    public int Progress { get; set; }
    public DateTime? CompletionTime { get; set; }
    public ulong ProfileId { get; set; }
    public ProfileEntity Profile { get; set; }
}
