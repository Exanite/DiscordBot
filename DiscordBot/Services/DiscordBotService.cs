using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class DiscordBotService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;
        private readonly LoggingService log;

        public DiscordBotService(IServiceProvider provider, DiscordSocketClient client, CommandService commands, DiscordBotConfig config, LoggingService log)
        {
            this.provider = provider;
            this.client = client;
            this.commands = commands;
            this.config = config;
            this.log = log;
        }

        public async Task Start()
        {
            if (String.IsNullOrEmpty(config.DiscordToken))
            {
                throw new Exception($"Please enter a Discord Bot token into the config file at {Program.ConfigPath}");
            }

            try
            {
                await client.LoginAsync(TokenType.Bot, config.DiscordToken);
                await client.StartAsync();
            }
            catch (HttpRequestException)
            {
                await log.Log(new LogMessage(LogSeverity.Error, "Discord", "Unable to connect to Discord servers."));
            }

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(), provider);
        }

        public async Task Stop()
        {
            await client.StopAsync();
        }
    }
}
