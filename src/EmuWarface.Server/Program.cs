using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EmuWarface.DAL.Repositories;
using EmuWarface.DAL;
using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmuWarface.Server.Common.Configuration;
using NLog;
using EmuWarface.Server.Game.Data.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;

public partial class Program
{
    static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static async Task Main(string[] args)
    {
        _logger.Info($"{nameof(EmuWarface)} v{Assembly.GetEntryAssembly().GetName().Version}");
        _logger.Info($"GitHub repository: https://github.com/n1kodim/EmuWarface");

        var host = Host.CreateDefaultBuilder(args);
        ConfigureHost(host);

        try
        {
            await host.RunConsoleAsync();
        }
        catch (Exception e)
        {
            _logger.Fatal(e);
        }
    }

    public static void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json", false);
            config.AddJsonFile("appsettings.dev.json", true, true);
        })
        .ConfigureLogging((ctx, configLogging) =>
        {
            configLogging.ClearProviders();
            configLogging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            //configLogging.AddNLog();
        })
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureServices(services =>
        {
            services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });
            //TODO: XmppSharp
            //services.AddHostedService<XmppServer>();
        })
        .ConfigureContainer<ContainerBuilder>((ctx, containerBuilder) =>
        {
            containerBuilder.Register(c => ctx.Configuration);

            var dbContext = new GameDbContextFactory()
                .CreateDbContext(ctx.Configuration.GetConnectionString("MySqlConnection"))
                .Migrate();

            containerBuilder.Register(c => dbContext)
                .InstancePerLifetimeScope();

            // DAL repositories
            containerBuilder.RegisterType<ProfileRepository>();

            containerBuilder.Register(c => ctx.Configuration.GetSection(GameOptions.Game).Get<GameOptions>()!);
            containerBuilder.Register(c => ctx.Configuration.GetSection(XmppOptions.Xmpp).Get<XmppOptions>()!);

            // scan an assembly for query handlers
            var assembly = Assembly.GetExecutingAssembly();

            // TODO: GameResources (GameConfigs) external with try catch

            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces()
                .Any(i => i.IsAssignableFrom(typeof(IGameConfig))))
                .SingleInstance()
                .As<IGameConfig>()
                .AsSelf()
                .AutoActivate()
                .OnActivated(c => ((IGameConfig)c.Instance).LoadAsync());

            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetCustomAttribute(typeof(ServiceAttribute)) != null)
                .SingleInstance()
                .AsSelf()
                .AutoActivate();

            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsSubclassOf(typeof(QueryHandler)))
                .SingleInstance()
                .AsSelf()
                .As<QueryHandler>();
        });
}