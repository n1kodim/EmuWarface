using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class ProfileItemConfiguration : IEntityTypeConfiguration<ProfileItemEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileItemEntity> builder)
        {
            builder
                .HasKey(i => i.Id);

            builder
                .Property(i => i.Name)
                .HasMaxLength(50);

            builder
                .Property(i => i.Config)
                .HasMaxLength(255);

            builder
                .HasOne(a => a.Profile)
                .WithMany(p => p.Items);
        }
    }
}
