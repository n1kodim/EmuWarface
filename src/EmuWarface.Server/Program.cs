using Autofac;
using EmuWarface.Common.Configuration;
using EmuWarface.Database;
using EmuWarface.Server.CryOnline;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Query;
using EmuWarface.Server.Data;
using EmuWarface.Server.Game;
using EmuWarface.Server.Game.Channel;
using EmuWarface.Server.Network;
using EmuWarface.Server.Query;
using EmuWarface.Server.Query.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;

namespace EmuWarface
{
    static class Program
	{
        static readonly Logger _logger = LogManager.GetLogger("Server");

        static async Task Main(string[] args)
		{
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            _logger.Info($"{nameof(EmuWarface)} v{Assembly.GetEntryAssembly().GetName().Version}");
            _logger.Info($"GitHub repository: https://github.com/n1kodim/EmuWarface");

            BuildServices();

            //_logger.Info(@"Done. For help, type ""/help""");

            await Task.Delay(Timeout.Infinite);
        }

        private static void BuildServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AppSettings>();

            builder.Register(c =>
                {
                    var config = c.Resolve<AppSettings>();
                    var db = new GameDatabase(config.ConnectionString);
                    db.Migrate();
                    return db;
                })
                .As<IGameDatabase>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameResources>()
                .SingleInstance();
            builder.RegisterType<ChannelManager>()
                .SingleInstance();

            // scan an assembly for query handlers
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<QueryAttribute>() != null);
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces().Any(x => x == typeof(IQueryHandler)))
                .AsImplementedInterfaces();

            builder.RegisterType<QueryFactory>()
                .SingleInstance();

            builder.RegisterType<XmppServer>()
                .SingleInstance()
                .AutoActivate()
                .OnActivated(x => x.Instance.Start());

            var container = builder.Build();

            //container.Resolve<IGameDatabase>().Migrate();
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Fatal((Exception)e.ExceptionObject);
            Console.WriteLine("Press ENTER to continue ...");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}