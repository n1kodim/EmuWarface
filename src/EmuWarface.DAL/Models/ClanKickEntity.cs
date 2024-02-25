namespace EmuWarface.DAL.Models;

public class ClanKickEntity
{
    public DateTime KickDate { get; set; } = DateTime.Now;
    
    public ulong ProfileId { get; set; }
    public ProfileEntity Profile { get; set; }
    public ulong ClanId { get; set; }
    public ClanEntity Clan { get; set; }
}