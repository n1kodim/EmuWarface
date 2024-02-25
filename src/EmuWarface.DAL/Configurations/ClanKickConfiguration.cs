using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class ClanKickConfiguration : IEntityTypeConfiguration<ClanKickEntity>
    {
        public void Configure(EntityTypeBuilder<ClanKickEntity> builder)
        {
            builder.HasKey(k => new { k.ProfileId, k.ClanId });

            builder
                .HasOne(k => k.Profile)
                .WithMany(p => p.ClanKicks);
        }
    }
}