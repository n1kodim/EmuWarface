namespace EmuWarface.DAL.Models;

public partial class ProfileItemEntity
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Config { get; set; }
    public ulong SlotIds { get; set; }
    public uint? Quantity { get; set; }
    public uint? DurabilityPoints { get; set; }
    public DateTime? ExpirationTime { get; set; }
    public DateTime BuyTime { get; set; }
    public ulong ProfileId { get; set; }
    public ProfileEntity Profile { get; set; }
}
