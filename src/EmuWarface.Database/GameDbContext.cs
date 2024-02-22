using EmuWarface.Common.Configuration;
using EmuWarface.Database.Model;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EmuWarface.Database
{
	public class GameDbContext : DbContext
	{
        public GameDbContext() : base() { }
		public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        private readonly string _connectionString;

        public DbSet<User> Users { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<ProfileItem> ProfileItems { get; set; }
		public DbSet<ProfileAchievement> ProfileAchievements { get; set; }

        public GameDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (!optionsBuilder.IsConfigured)
			{
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString), b =>
                {
                    b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("profiles");

                entity.HasIndex(e => e.UserId, "ix_profiles_user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("id");
                entity.Property(e => e.BanExpire)
                    .HasMaxLength(6)
                    .HasColumnName("ban_expire");
                entity.Property(e => e.BannerBadge)
                    .HasDefaultValueSql("'4294967295'")
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("banner_badge");
                entity.Property(e => e.BannerMark)
                    .HasDefaultValueSql("'4294967295'")
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("banner_mark");
                entity.Property(e => e.BannerStripe)
                    .HasDefaultValueSql("'4294967295'")
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("banner_stripe");
                entity.Property(e => e.CreateTime)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("current_timestamp(6)")
                    .HasColumnName("create_time");
                entity.Property(e => e.CurrentClass)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("current_class");
                entity.Property(e => e.Experience)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("experience");
                entity.Property(e => e.Fatness).HasColumnName("fatness");
                entity.Property(e => e.Head).HasColumnName("head");
                entity.Property(e => e.Height)
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("height");
                entity.Property(e => e.LastSeen)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("current_timestamp(6)")
                    .HasColumnName("last_seen");
                entity.Property(e => e.MissionPassed)
                    .HasColumnType("tinyint(3) unsigned")
                    .HasColumnName("mission_passed");
                entity.Property(e => e.MuteExpire)
                    .HasMaxLength(6)
                    .HasColumnName("mute_expire");
                entity.Property(e => e.Nickname)
                    .HasMaxLength(16)
                    .HasColumnName("nickname");
                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_profiles_users_user_id");
            });

            modelBuilder.Entity<ProfileAchievement>(entity =>
            {
                entity.HasKey(e => e.AchievementId).HasName("PRIMARY");

                entity.ToTable("profile_achievements");

                entity.HasIndex(e => e.ProfileId, "ix_profile_achievements_profile_id");

                entity.Property(e => e.AchievementId)
                    .HasColumnType("int(11)")
                    .HasColumnName("achievement_id");
                entity.Property(e => e.CompletionTime)
                    .HasMaxLength(6)
                    .HasColumnName("completion_time");
                entity.Property(e => e.ProfileId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("profile_id");
                entity.Property(e => e.Progress)
                    .HasColumnType("int(11)")
                    .HasColumnName("progress");

                entity.HasOne(d => d.Profile).WithMany(p => p.ProfileAchievements)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("fk_profile_achievements_profiles_profile_id");
            });

            modelBuilder.Entity<ProfileItem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("profile_items");

                entity.HasIndex(e => e.ProfileId, "ix_profile_items_profile_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("id");
                entity.Property(e => e.AttachedTo)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("attached_to");
                entity.Property(e => e.BuyTime)
                    .HasMaxLength(6)
                    .HasColumnName("buy_time");
                entity.Property(e => e.CatalogId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("catalog_id");
                entity.Property(e => e.Config).HasColumnName("config");
                entity.Property(e => e.DurabilityPoints)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("durability_points");
                entity.Property(e => e.ExpirationTime)
                    .HasMaxLength(6)
                    .HasColumnName("expiration_time");
                entity.Property(e => e.Item)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("item");
                entity.Property(e => e.ProfileId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("profile_id");
                entity.Property(e => e.SlotIds)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("slot_ids");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.TotalDurabilityPoints)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("total_durability_points");

                entity.HasOne(d => d.Profile).WithMany(p => p.ProfileItems)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("fk_profile_items_profiles_profile_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.ProfileId, "ix_users_profile_id");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("id");
                entity.Property(e => e.CreateTime)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("current_timestamp(6)")
                    .HasColumnName("create_time");
                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsFixedLength()
                    .HasColumnName("password");
                entity.Property(e => e.ProfileId)
                    .HasColumnType("bigint(20) unsigned")
                    .HasColumnName("profile_id");
                entity.Property(e => e.Username)
                    .HasMaxLength(16)
                    .HasColumnName("username");

                entity.HasOne(d => d.Profile).WithMany(p => p.Users)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("fk_users_profiles_profile_id");
            });
        }
    }
}
