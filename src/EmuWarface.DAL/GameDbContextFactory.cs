using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmuWarface.DAL
{
    public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        public GameDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();

            optionsBuilder
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .UseSnakeCaseNamingConvention();

            return new GameDbContext(optionsBuilder.Options);
        }

        public GameDbContext CreateDbContext(string[] args)
        {
            // TODO: do not use a temporary connection string for migrations
            return CreateDbContext("server=127.0.0.1;port=3306;user=root;password=;database=emuwarface");
        }
    }
}