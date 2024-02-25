using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EmuWarface.DAL.Configurations
{
    public class ClanMemberConfiguration : IEntityTypeConfiguration<ClanMemberEntity>
    {
        public void Configure(EntityTypeBuilder<ClanMemberEntity> builder)
        {
            builder.HasKey(c => c.ProfileId);

            builder
                .HasOne(c => c.Clan)
                .WithMany(p => p.Members)
                .HasForeignKey(c => c.ClanId);

            builder
                .HasOne(c => c.Profile)
                .WithOne(p => p.ClanMember);
        }
    }
}
