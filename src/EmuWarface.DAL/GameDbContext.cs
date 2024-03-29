﻿using EmuWarface.DAL.Configurations;
using EmuWarface.DAL.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EmuWarface.DAL
{
	public class GameDbContext : DbContext
	{
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public DbSet<UserEntity> Users { get; set; }
		public DbSet<ProfileEntity> Profiles { get; set; }
		public DbSet<ProfileItemEntity> ProfileItems { get; set; }
		public DbSet<ProfileAchievementEntity> ProfileAchievements { get; set; }
		public DbSet<ClanEntity> Clans { get; set; }
		public DbSet<ClanKickEntity> ClanKicks { get; set; }
		public DbSet<ClanMemberEntity> ClanMembers { get; set; }
		public DbSet<FriendEntity> Friends { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options) 
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public GameDbContext Migrate()
        {
            List<string> migrations = Database.GetPendingMigrations().ToList();
            if (migrations.Count > 0)
            {
                _logger.Info($"Applying {migrations.Count} migration(s) to game database...");
                foreach (string migration in migrations)
                    _logger.Info(migration);
                Database.Migrate();
            }
            _logger.Info("Connected to game database successfully. ({0})", ServerVersion.AutoDetect(Database.GetConnectionString()));
            return this;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // profile configs
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileAchievementConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileItemConfiguration());

            // clan configs
            modelBuilder.ApplyConfiguration(new ClanConfiguration());
            modelBuilder.ApplyConfiguration(new ClanMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ClanKickConfiguration());

            modelBuilder.ApplyConfiguration(new FriendConfiguration());

            //base.OnModelCreating(modelBuilder);
            /*modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<ProfileEntity>(entity =>
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

            modelBuilder.Entity<ProfileAchievementEntity>(entity =>
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

            modelBuilder.Entity<ProfileItemEntity>(entity =>
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

            modelBuilder.Entity<UserEntity>(entity =>
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
            });*/
        }
    }
}
