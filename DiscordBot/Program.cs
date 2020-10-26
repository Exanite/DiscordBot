using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Infiltrator;
using DiscordBot.Json;
using DiscordBot.Logging;
using DiscordBot.Logging.Serilog;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DiscordBot
{
    public class Program
    {
        public const string ConfigPath = @"Configs\DiscordBot\Config.json";

        private IContainer Container { get; set; }

        public static void Main()
        {
            try
            {
                var program = new Program();

                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    program.Exit();
                };

                program.MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        public async Task MainAsync()
        {
            Container = CreateContainer();

            Container.Resolve<CommandHandler>();
            await Container.Resolve<DiscordBotService>().Start();

            await Task.Delay(-1);
        }

        public void Exit()
        {
            if (Container != null)
            {
                var bot = Container.Resolve<DiscordBotService>();
                bot.Stop().GetAwaiter().GetResult();
            }

            Environment.Exit(0);
        }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            InstallProgram(builder);
            InstallLog(builder);
            InstallDiscordBot(builder);
            InstallInfiltratorGame(builder);
            InstallMiscellaneous(builder);

            return builder.Build();
        }

        private void InstallProgram(ContainerBuilder builder)
        {
            builder.Populate(new ServiceCollection());
        }

        private void InstallLog(ContainerBuilder builder)
        {
            builder.RegisterType<LoggingService>().SingleInstance();
            builder.Register(x => x.Resolve<LoggingService>().Logger).As<ILogger>().SingleInstance();

            builder.RegisterType<SerilogLogAdapter>().As<ILog>();
            builder.RegisterGeneric(typeof(SerilogLogAdapter<>)).As(typeof(ILog<>)).SingleInstance();
        }

        private void InstallDiscordBot(ContainerBuilder builder)
        {
            var reader = new JsonReader<DiscordBotConfig>(AppContext.BaseDirectory, ConfigPath);
            var config = reader.Load(true);

            var client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = config.Log.LogLevel,
                MessageCacheSize = config.Socket.MessageCacheSize,
                AlwaysDownloadUsers = config.Socket.AlwaysDownloadUsers,
                ConnectionTimeout = config.Socket.ConnectionTimeOut,
                DefaultRetryMode = config.Socket.DefaultRetryMode,
            });

            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = config.Log.LogLevel,
                CaseSensitiveCommands = config.Commands.CaseSensitive,
                DefaultRunMode = config.Commands.DefaultRunMode,
            });

            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterInstance(reader).SingleInstance();

            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterInstance(commandService).SingleInstance();

            builder.RegisterType<CommandHandler>().SingleInstance();
            builder.RegisterType<DiscordBotService>().SingleInstance();
            builder.RegisterType<EmbedHelper>().SingleInstance();
        }

        private void InstallInfiltratorGame(ContainerBuilder builder)
        {
            builder.RegisterType<InfiltratorGameManager>().SingleInstance();
            builder.RegisterType<InfiltratorGame.Factory>().SingleInstance();
            builder.RegisterType<Enemy.Factory>().SingleInstance();
        }

        private void InstallMiscellaneous(ContainerBuilder builder)
        {
            builder.RegisterType<Random>().SingleInstance();
        }
    }
}
