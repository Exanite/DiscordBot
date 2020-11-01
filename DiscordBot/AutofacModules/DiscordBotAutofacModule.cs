using System;
using Autofac;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core;
using DiscordBot.Core.Configuration;
using DiscordBot.Core.Json;
using DiscordBot.Core.Logging.Discord;
using DiscordBot.Core.Services;

namespace DiscordBot.AutofacModules
{
    public class DiscordBotAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var reader = new JsonReader<DiscordBotConfig>(AppContext.BaseDirectory, Constants.ConfigFilePath);
            var config = reader.Load(true);

            var client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = config.Log.LogLevel.ToLogSeverity(),
                MessageCacheSize = config.Socket.MessageCacheSize,
                AlwaysDownloadUsers = config.Socket.AlwaysDownloadUsers,
                ConnectionTimeout = config.Socket.ConnectionTimeOut,
                DefaultRetryMode = config.Socket.DefaultRetryMode,
            });

            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = config.Log.LogLevel.ToLogSeverity(),
                CaseSensitiveCommands = config.Commands.CaseSensitive,
                DefaultRunMode = config.Commands.DefaultRunMode,
            });

            builder.RegisterInstance(config).SingleInstance();
            builder.RegisterInstance(reader).SingleInstance();

            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterInstance(commandService).SingleInstance();

            builder.RegisterType<DiscordLoggingService>().SingleInstance();
            builder.RegisterType<CommandHandler>().SingleInstance();
            builder.RegisterType<DiscordBotService>().SingleInstance();
            builder.RegisterType<EmbedHelper>().SingleInstance();
        }
    }
}
