using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class ClanConfiguration : IEntityTypeConfiguration<ClanEntity>
    {
        public void Configure(EntityTypeBuilder<ClanEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasIndex(c => new { c.Name, c.MasterProfileId })
                .IsUnique();

            builder
                .Property(c => c.Name)
                .HasMaxLength(20);

            builder
                .Property(c => c.Description)
                .HasMaxLength(8192);

            builder
                .HasMany(c => c.Members)
                .WithOne(p => p.Clan)
                .HasForeignKey(k => k.ProfileId);
        }
    }
}
