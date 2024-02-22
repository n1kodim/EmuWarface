using EmuWarface.Database.Model;

namespace EmuWarface.Database
{
    public interface IGameDatabase
    {
        void Migrate();
        Task<bool> HasProfileAsync(ulong userid);
        Task<Profile?> GetProfileByUserIdAsync(ulong userid);
    }
}
