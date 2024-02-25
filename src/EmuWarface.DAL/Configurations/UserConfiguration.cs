using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmuWarface.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasIndex(p => p.Username)
                .IsUnique();
            builder
                .Property(u => u.Username)
                .HasMaxLength(16);

            builder
                .Property(u => u.Password)
                .HasMaxLength(64);

            builder
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserEntity>(p => p.Id);
        }
    }
}
