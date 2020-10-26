using System;
using System.IO;
using DiscordBot.Configuration;
using DiscordBot.Logging.Serilog;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;

namespace DiscordBot.Services
{
    public class LoggingService
    {
        private ILogger logger;

        private readonly DiscordBotConfig config;

        public LoggingService(DiscordBotConfig config)
        {
            this.config = config;
        }

        public ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    logger = CreateLogger();
                }

                return logger;
            }
        }

        private string OutputTemplate { get; } = "[{Timestamp:HH:mm:ss}] [{Level}] [{ShortContext}]: {Message:lj}{NewLine}{Exception}";

        private ILogger CreateLogger()
        {
            string path = GetNewLogFilePath();

            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

            var config = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .Enrich.With<ShortContextEnricher>()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.ControlledBy(levelSwitch);

            WriteToFile(config, path, OutputTemplate);
            WriteToConsole(config, OutputTemplate);

            return config.CreateLogger();
        }

        private string GetNewLogFilePath()
        {
            string LogFileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log";

            return Path.Combine(config.Log.LogFolderPath, LogFileName);
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
    }
}