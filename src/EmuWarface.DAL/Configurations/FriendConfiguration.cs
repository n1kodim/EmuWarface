using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<FriendEntity>
    {
        public void Configure(EntityTypeBuilder<FriendEntity> builder)
        {
            builder.HasKey(f => new { f.ProfileId, f.FriendId });

            builder
                .HasOne(f => f.Profile)
                .WithMany(p => p.Friends)
                .HasForeignKey(p => p.ProfileId);

            builder
                .HasOne(p => p.FriendProfile)
                .WithMany()
                .HasForeignKey(f => f.FriendId);
        }
    }
}
