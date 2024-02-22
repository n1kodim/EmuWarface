using Microsoft.Extensions.Configuration;

namespace EmuWarface.Common.Configuration
{
    public class AppSettings
    {
        public XmppServerSettings XmppServer { get; set; }
        public string ConnectionString { get; set; }

        public AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.dev.json", optional: true);

            var root = builder.Build();

            // TODO: Auto detect DB type
            ConnectionString = root.GetConnectionString("MySqlConnection");

            root.Bind(this);
        }
    }
}
