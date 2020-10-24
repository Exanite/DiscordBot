using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Infiltrator;
using DiscordBot.Json;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        public const string ConfigPath = @"Configs\DiscordBot\Config.json";

        private IServiceProvider ServiceProvider { get; set; }

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
            ServiceProvider = ConfigureServices();
            ServiceProvider.GetRequiredService<CommandHandler>();
            ServiceProvider.GetRequiredService<LoggingService>();
            await ServiceProvider.GetRequiredService<DiscordBotService>().Start();

            await Task.Delay(-1);
        }

        public void Exit()
        {
            if (ServiceProvider != null)
            {
                var bot = ServiceProvider.GetService<DiscordBotService>();
                bot.Stop().GetAwaiter().GetResult();
            }

            Environment.Exit(0);
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            InstallDiscordBot(services);
            InstallInfiltratorGame(services);

            return services.BuildServiceProvider();
        }

        private void InstallDiscordBot(ServiceCollection services)
        {
            var reader = new JsonReader<DiscordBotConfig>(AppContext.BaseDirectory, ConfigPath);
            var config = reader.Load(true);

            services
                .AddSingleton(new JsonReader<DiscordBotConfig>(AppContext.BaseDirectory, ConfigPath))
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = config.Log.LogLevel,
                    MessageCacheSize = config.Socket.MessageCacheSize,
                    AlwaysDownloadUsers = config.Socket.AlwaysDownloadUsers,
                    ConnectionTimeout = config.Socket.ConnectionTimeOut,
                    DefaultRetryMode = config.Socket.DefaultRetryMode,
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = config.Log.LogLevel,
                    CaseSensitiveCommands = config.Commands.CaseSensitive,
                    DefaultRunMode = config.Commands.DefaultRunMode,
                }))
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<DiscordBotService>()
                .AddSingleton<EmbedHelper>()
                .AddSingleton(config)
                .AddSingleton(reader);
        }

        private void InstallInfiltratorGame(ServiceCollection services)
        {
            services
                .AddSingleton<InfiltratorGameManager>()
                .AddSingleton<InfiltratorGame.Factory>();
        }
    }
}

