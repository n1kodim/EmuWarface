using EmuWarface.Database.Model;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EmuWarface.Database
{
    public class GameDatabase : IGameDatabase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string _connectionString;

        public GameDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Migrate()
        {
            using var context = new GameDbContext(_connectionString);

            List<string> migrations = context.Database.GetPendingMigrations().ToList();
            if (migrations.Count > 0)
            {
                _logger.Info($"Applying {migrations.Count} database migration(s)...");
                foreach (string migration in migrations)
                    _logger.Info(migration);

                context.Database.Migrate();
            }

            _logger.Info("Connected to database successfully. ({0})", ServerVersion.AutoDetect(context.Database.GetConnectionString()));
        }

        public async Task<bool> HasProfileAsync(ulong userid)
        {
            using var context = new GameDbContext(_connectionString);
            return await context.Profiles.
                SingleOrDefaultAsync(x => x.User.Id == userid) != null;
        }

        public async Task<Profile?> GetProfileByUserIdAsync(ulong userid)
        {
            using var context = new GameDbContext(_connectionString);
            return await context.Profiles
                .SingleOrDefaultAsync(x => x.UserId == userid);
        }
    }
}
