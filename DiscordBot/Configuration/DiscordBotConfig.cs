using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DiscordBot.Configuration
{
    [Serializable]
    public class DiscordBotConfig
    {
        public string DiscordToken { get; set; }

        public LogConfig Log { get; set; } = new LogConfig();
        public CommandsConfig Commands { get; set; } = new CommandsConfig();
        public SocketConfig Socket { get; set; } = new SocketConfig();

        [Serializable]
        public class LogConfig
        {
            public string LocalLogFolderPath { get; set; } = @"Logs";
            [JsonConverter(typeof(StringEnumConverter))]
            public LogSeverity LogLevel { get; set; } = LogSeverity.Verbose;
        }

        [Serializable]
        public class CommandsConfig
        {
            public string Prefix { get; set; } = "&";
            public bool CaseSensitive { get; set; } = false;
            [JsonConverter(typeof(StringEnumConverter))]
            public RunMode DefaultRunMode { get; set; } = RunMode.Async;
            public bool ShowErrorMessages { get; set; } = true;
            public bool DeleteErrorMessages { get; set; } = true;
            public int ErrorMessageDeletionTime { get; set; } = 30;
        }

        [Serializable]
        public class SocketConfig
        {
            public int MessageCacheSize { get; set; } = 1000;
            public bool AlwaysDownloadUsers { get; set; } = false;
            public int ConnectionTimeOut { get; set; } = 30000;
            [JsonConverter(typeof(StringEnumConverter))]
            public RetryMode DefaultRetryMode { get; set; } = RetryMode.AlwaysRetry;
        }
    }
}