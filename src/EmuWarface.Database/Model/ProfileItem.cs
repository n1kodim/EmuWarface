using System;
using System.Collections.Generic;

namespace EmuWarface.Database.Model;

public partial class ProfileItem
{
    public ulong Id { get; set; }

    public ulong ProfileId { get; set; }

    public ulong Item { get; set; }

    public ulong AttachedTo { get; set; }

    public ulong? SlotIds { get; set; }

    public string Config { get; set; } = null!;

    public string Status { get; set; } = null!;

    public ulong CatalogId { get; set; }

    public DateTime BuyTime { get; set; }

    public DateTime ExpirationTime { get; set; }

    public uint TotalDurabilityPoints { get; set; }

    public uint DurabilityPoints { get; set; }

    public virtual Profile Profile { get; set; } = null!;
}
