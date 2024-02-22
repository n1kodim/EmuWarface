using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmuWarface.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profile_achievements",
                columns: table => new
                {
                    achievementid = table.Column<int>(name: "achievement_id", type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileid = table.Column<ulong>(name: "profile_id", type: "bigint(20) unsigned", nullable: false),
                    progress = table.Column<int>(type: "int(11)", nullable: false),
                    completiontime = table.Column<DateTime>(name: "completion_time", type: "datetime(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.achievementid);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "profile_items",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileid = table.Column<ulong>(name: "profile_id", type: "bigint(20) unsigned", nullable: false),
                    item = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                    attachedto = table.Column<ulong>(name: "attached_to", type: "bigint(20) unsigned", nullable: false),
                    slotids = table.Column<ulong>(name: "slot_ids", type: "bigint(20) unsigned", nullable: true),
                    config = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    catalogid = table.Column<ulong>(name: "catalog_id", type: "bigint(20) unsigned", nullable: false),
                    buytime = table.Column<DateTime>(name: "buy_time", type: "datetime(6)", maxLength: 6, nullable: false),
                    expirationtime = table.Column<DateTime>(name: "expiration_time", type: "datetime(6)", maxLength: 6, nullable: false),
                    totaldurabilitypoints = table.Column<uint>(name: "total_durability_points", type: "int(10) unsigned", nullable: false),
                    durabilitypoints = table.Column<uint>(name: "durability_points", type: "int(10) unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userid = table.Column<ulong>(name: "user_id", type: "bigint(20) unsigned", nullable: false),
                    nickname = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    height = table.Column<float>(type: "float", nullable: false, defaultValueSql: "'1'"),
                    fatness = table.Column<float>(type: "float", nullable: false),
                    head = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    currentclass = table.Column<uint>(name: "current_class", type: "int(10) unsigned", nullable: true),
                    experience = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false),
                    missionpassed = table.Column<byte>(name: "mission_passed", type: "tinyint(3) unsigned", nullable: false),
                    bannerbadge = table.Column<uint>(name: "banner_badge", type: "int(10) unsigned", nullable: false, defaultValueSql: "'4294967295'"),
                    bannermark = table.Column<uint>(name: "banner_mark", type: "int(10) unsigned", nullable: false, defaultValueSql: "'4294967295'"),
                    bannerstripe = table.Column<uint>(name: "banner_stripe", type: "int(10) unsigned", nullable: false, defaultValueSql: "'4294967295'"),
                    lastseen = table.Column<DateTime>(name: "last_seen", type: "datetime(6)", maxLength: 6, nullable: false, defaultValueSql: "current_timestamp(6)"),
                    createtime = table.Column<DateTime>(name: "create_time", type: "datetime(6)", maxLength: 6, nullable: false, defaultValueSql: "current_timestamp(6)"),
                    banexpire = table.Column<DateTime>(name: "ban_expire", type: "datetime(6)", maxLength: 6, nullable: false),
                    muteexpire = table.Column<DateTime>(name: "mute_expire", type: "datetime(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileid = table.Column<ulong>(name: "profile_id", type: "bigint(20) unsigned", nullable: false),
                    username = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "char(64)", fixedLength: true, maxLength: 64, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createtime = table.Column<DateTime>(name: "create_time", type: "datetime(6)", maxLength: 6, nullable: false, defaultValueSql: "current_timestamp(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_profiles_profile_id",
                        column: x => x.profileid,
                        principalTable: "profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "ix_profile_achievements_profile_id",
                table: "profile_achievements",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_items_profile_id",
                table: "profile_items",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_profiles_user_id",
                table: "profiles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_profile_id",
                table: "users",
                column: "profile_id");

            migrationBuilder.AddForeignKey(
                name: "fk_profile_achievements_profiles_profile_id",
                table: "profile_achievements",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_profile_items_profiles_profile_id",
                table: "profile_items",
                column: "profile_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_profiles_users_user_id",
                table: "profiles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_profiles_profile_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "profile_achievements");

            migrationBuilder.DropTable(
                name: "profile_items");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
