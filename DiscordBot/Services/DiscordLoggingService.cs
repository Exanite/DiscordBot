using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Logging;
using DiscordBot.Logging.Discord;
using DiscordBot.Logging.Serilog;

namespace DiscordBot.Services
{
    public class DiscordLoggingService
    {
        public const string CommandSourceContext = "Command";
        private const string DiscordSourceContext = "Discord";
        private const string GatewaySourceContext = "Gateway";
        private const string RestSourceContext = "Rest";

        private ILog defaultLog;
        private ILog commandLog;
        private ILog discordLog;
        private ILog gatewayLog;
        private ILog restLog;

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
            ILog log = defaultLog;

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
