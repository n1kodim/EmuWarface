namespace EmuWarface.DAL.Models;

public class ClanMemberEntity
{
    public ulong Points { get; set; }
    public ClanMemberRole Role { get; set; } = ClanMemberRole.Regular;
    public DateTime InvitationDate { get; set; } = DateTime.Now;

    public ulong ProfileId { get; set; }
    public ProfileEntity Profile { get; set; }
    public ulong ClanId { get; set; }
    public ClanEntity Clan { get; set; }
}