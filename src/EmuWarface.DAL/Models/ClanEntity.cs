namespace EmuWarface.DAL.Models;

public class ClanEntity
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public List<ClanMemberEntity> Members { get; set; } = new List<ClanMemberEntity>();
    public ulong MasterProfileId { get; set; }
    public ProfileEntity MasterProfile { get; set; }
}