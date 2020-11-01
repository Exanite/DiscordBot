using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using DiscordBot.Logging;

namespace DiscordBot.Services
{
    public class DiscordBotService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;
        private readonly ILog log;

        public DiscordBotService(IServiceProvider provider, DiscordSocketClient client, CommandService commands, DiscordBotConfig config, ILog<DiscordBotService> log)
        {
            this.provider = provider;
            this.client = client;
            this.commands = commands;
            this.config = config;
            this.log = log;
        }

        public async Task Start()
        {
            if (string.IsNullOrEmpty(config.DiscordToken))
            {
                throw new Exception($"Please enter a Discord Bot token into the config file at "); // ! {Program.ConfigFilePath}
            }

            try
            {
                await client.LoginAsync(TokenType.Bot, config.DiscordToken);
                await client.StartAsync();
            }
            catch (HttpRequestException)
            {
                log.Fatal("Unable to connect to Discord servers.");
            }

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(), provider);
        }

        public async Task Stop()
        {
            await client.StopAsync();
        }
    }
}
