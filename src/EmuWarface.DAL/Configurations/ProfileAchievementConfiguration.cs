using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class ProfileAchievementConfiguration : IEntityTypeConfiguration<ProfileAchievementEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileAchievementEntity> builder)
        {
            builder
                .HasKey(a => new { a.Id, a.ProfileId });

            builder
                .HasOne(a => a.Profile)
                .WithMany(p => p.Achievements)
                .HasForeignKey(f => f.ProfileId);
        }
    }
}
