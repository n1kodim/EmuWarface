using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmuWarface.DAL.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasIndex(p => p.Nickname)
                .IsUnique();

            builder
                .Property(p => p.Head)
                .HasMaxLength(20);

            builder
                .Property(p => p.Nickname)
                .HasMaxLength(16);

            builder
               .HasOne(u => u.User)
               .WithOne(p => p.Profile)
               .HasForeignKey<ProfileEntity>(p => p.UserId);

            builder
                .HasMany(a => a.Achievements)
                .WithOne(p => p.Profile)
                .HasForeignKey(p => p.ProfileId);

            builder
                .HasMany(a => a.Items)
                .WithOne(p => p.Profile)
                .HasForeignKey(p => p.ProfileId);

            builder
                .HasOne(p => p.ClanMember)
                .WithOne(cm => cm.Profile)
                .HasForeignKey<ClanMemberEntity>(p => p.ProfileId);

            builder
                .HasMany(ck => ck.ClanKicks)
                .WithOne(p => p.Profile)
                .HasForeignKey(f => f.ProfileId);
        }
    }
}
