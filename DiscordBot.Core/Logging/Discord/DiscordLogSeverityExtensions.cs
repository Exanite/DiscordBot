using System;
using Discord;

namespace DiscordBot.Core.Logging.Discord
{
    /// <summary>
    /// Extensions for Discord's <see cref="LogSeverity"/>
    /// </summary>
    public static class DiscordLogSeverityExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogLevel"/> to a <see cref="LogType"/>
        /// </summary>
        public static LogSeverity ToLogSeverity(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose: return LogSeverity.Verbose;
                case LogLevel.Debug: return LogSeverity.Debug;
                case LogLevel.Information: return LogSeverity.Info;
                case LogLevel.Warning: return LogSeverity.Warning;
                case LogLevel.Error: return LogSeverity.Error;
                case LogLevel.Fatal: return LogSeverity.Critical;
                default: throw new NotSupportedException($"{level} is not a supported {typeof(LogLevel).Name}");
            }
        }

        /// <summary>
        /// Converts a <see cref="LogType"/> to a <see cref="LogLevel"/>
        /// </summary>
        public static LogLevel ToLogLevel(this LogSeverity level)
        {
            switch (level)
            {
                case LogSeverity.Verbose: return LogLevel.Verbose;
                case LogSeverity.Debug: return LogLevel.Debug;
                case LogSeverity.Info: return LogLevel.Information;
                case LogSeverity.Warning: return LogLevel.Warning;
                case LogSeverity.Error: return LogLevel.Error;
                case LogSeverity.Critical: return LogLevel.Fatal;
                default: throw new NotSupportedException($"{level} is not a supported {typeof(LogSeverity).Name}");
            }
        }
    }
}
