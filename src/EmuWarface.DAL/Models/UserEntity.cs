namespace EmuWarface.DAL.Models;

public class UserEntity
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public ulong ProfileId { get; set; }
    public ProfileEntity? Profile { get; set; }
}
