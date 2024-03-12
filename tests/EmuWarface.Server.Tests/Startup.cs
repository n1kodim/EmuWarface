using Microsoft.Extensions.Hosting;

namespace EmuWarface.Server.Tests
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) => Program.ConfigureHost(hostBuilder);
    }
}
