using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;

        private string LogFileName { get; set; }

        private string LogFolderPath => Path.Combine(AppContext.BaseDirectory, config.Log.RelativeLogFolderPath);
        private string LogFilePath => Path.Combine(LogFolderPath, LogFileName);

        public LoggingService(DiscordSocketClient client, CommandService commands, DiscordBotConfig config)
        {
            this.client = client;
            this.commands = commands;
            this.config = config;

            client.Log += Log;
            commands.Log += Log;

            StartNewLog();
        }

        public void StartNewLog()
        {
            LogFileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log";
        }

        public async Task Log(LogMessage message)
        {
            var directory = new DirectoryInfo(LogFolderPath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            var logFile = new FileInfo(LogFilePath);
            if (!logFile.Exists)
            {
                logFile.Create().Dispose();
            }

            string logMessage = $"[{DateTime.UtcNow:HH:mm:ss}] [{message.Severity}] [{message.Source}]: {message.Message}";

            if (message.Exception != null)
            {
                logMessage += $"{Environment.NewLine}{message.Exception}";
            }

            await File.AppendAllTextAsync(LogFilePath, $"{logMessage}{Environment.NewLine}");

            await Console.Out.WriteLineAsync(logMessage);
        }
    }
}