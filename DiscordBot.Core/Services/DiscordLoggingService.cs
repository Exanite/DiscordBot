using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Logging;
using DiscordBot.Core.Logging.Discord;
using DiscordBot.Core.Logging.Serilog;

namespace DiscordBot.Core.Services
{
    public class DiscordLoggingService
    {
        private const string CommandSourceContext = "Command";
        private const string DiscordSourceContext = "Discord";
        private const string GatewaySourceContext = "Gateway";
        private const string RestSourceContext = "Rest";

        private readonly ILog defaultLog;
        private readonly ILog commandLog;
        private readonly ILog discordLog;
        private readonly ILog gatewayLog;
        private readonly ILog restLog;

        public DiscordLoggingService(ILog log, DiscordSocketClient client, CommandService commands)
        {
            defaultLog = log;
            commandLog = log.ForContext(ShortContextEnricher.SourceContext, CommandSourceContext);
            discordLog = log.ForContext(ShortContextEnricher.SourceContext, DiscordSourceContext);
            gatewayLog = log.ForContext(ShortContextEnricher.SourceContext, GatewaySourceContext);
            restLog = log.ForContext(ShortContextEnricher.SourceContext, RestSourceContext);

            client.Log += Log;
            commands.Log += Log;
        }

        private Task Log(LogMessage message)
        {
            var log = defaultLog;

            switch (message.Source)
            {
                case CommandSourceContext: log = commandLog; break;
                case DiscordSourceContext: log = discordLog; break;
                case GatewaySourceContext: log = gatewayLog; break;
                case RestSourceContext: log = restLog; break;
            }

            log.Write(message.Severity.ToLogLevel(), message.Exception, message.Message);

            return Task.CompletedTask;
        }
    }
}
