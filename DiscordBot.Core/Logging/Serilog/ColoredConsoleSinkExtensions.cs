using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace DiscordBot.Logging.Serilog
{

    public static class ColoredConsoleSinkExtensions
    {
        public static LoggerConfiguration ColoredConsole(
            this LoggerSinkConfiguration loggerConfiguration,
            ITextFormatter formatter,
            LogEventLevel minimumLevel = LogEventLevel.Verbose)
        {
            return loggerConfiguration.Sink(new ColoredConsoleSink(formatter), minimumLevel);
        }
    }
}
