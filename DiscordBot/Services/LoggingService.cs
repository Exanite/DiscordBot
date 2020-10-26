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
        public LoggingService(DiscordSocketClient client, CommandService commands, DiscordBotConfig config)
        {
            Client = client;
            Commands = commands;
            Config = config;

            Client.Log += Log;
            Commands.Log += Log;

            StartNewLog();
        }

        private string LogFileName { get; set; }

        private string LogFolderPath => Path.Combine(AppContext.BaseDirectory, Config.Log.RelativeLogFolderPath);
        private string LogFilePath => Path.Combine(LogFolderPath, LogFileName);

        private DiscordSocketClient Client { get; }
        private CommandService Commands { get; }
        private DiscordBotConfig Config { get; }

        public void StartNewLog()
        {
            LogFileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.log";
        }

        public Task Log(LogMessage message)
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

            File.AppendAllText(LogFilePath, $"{logMessage}{Environment.NewLine}");

            Console.WriteLine(logMessage);

            return Task.CompletedTask;
        }
    }
}