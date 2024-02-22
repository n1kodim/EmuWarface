using System;
using System.Collections.Generic;

namespace EmuWarface.Database.Model;

public partial class User
{
    public ulong Id { get; set; }

    public ulong ProfileId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; } = new List<Profile>();
}
