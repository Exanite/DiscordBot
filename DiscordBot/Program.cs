using System;
using System.IO;
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
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;

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
            Container.Resolve<LoggingService>();
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
            string outputTemplate = "[{Timestamp:HH:mm:ss}] [{Level}] [{ShortContext}]: {Message:lj}{NewLine}{Exception}";
            string path = Path.Combine("Logs", "TheLog.log");

            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

            var config = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .Enrich.With<ShortContextEnricher>()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.ControlledBy(levelSwitch);

            WriteToFile(config, path, outputTemplate);
            WriteToConsole(config, outputTemplate);

            var log = config.CreateLogger();

            builder.RegisterInstance(log).As<ILogger>().SingleInstance();
            builder.RegisterGeneric(typeof(SerilogLogAdapter<>)).As(typeof(ILog<>)).SingleInstance();
        }

        private void WriteToConsole(LoggerConfiguration config, string outputTemplate)
        {
            var textFormatter = new MessageTemplateTextFormatter(outputTemplate);

            config.WriteTo.ColoredConsole(textFormatter);
        }

        private void WriteToFile(LoggerConfiguration config, string path, string outputTemplate)
        {
            var textFormatter = new MessageTemplateTextFormatter(outputTemplate);
            var jsonFormatter = new JsonFormatter();

            config.WriteTo.File(textFormatter, path);
            config.WriteTo.File(jsonFormatter, $"{path}.json");
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
            builder.RegisterType<LoggingService>().SingleInstance();
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
