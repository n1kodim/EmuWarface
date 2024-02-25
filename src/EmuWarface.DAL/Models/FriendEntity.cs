namespace EmuWarface.DAL.Models;

public class FriendEntity
{
    public ulong ProfileId { get; set; }
    public ProfileEntity Profile { get; set; }
    public ulong FriendId { get; set; }
    public ProfileEntity FriendProfile { get; set; }
}