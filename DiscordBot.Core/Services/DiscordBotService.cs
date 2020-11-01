using System;
using System.Collections.Generic;
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
        private readonly IEnumerable<Assembly> assemblies;
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;
        private readonly ILog log;

        public DiscordBotService(IEnumerable<Assembly> assemblies, IServiceProvider provider, DiscordSocketClient client, CommandService commands, DiscordBotConfig config, ILog<DiscordBotService> log)
        {
            this.assemblies = assemblies;
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

            foreach (var assembly in assemblies)
            {
                await commands.AddModulesAsync(assembly, provider);
            }
        }

        public async Task Stop()
        {
            await client.StopAsync();
        }
    }
}
