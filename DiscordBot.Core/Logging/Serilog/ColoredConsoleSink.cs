using System;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace DiscordBot.Logging.Serilog
{
    public class ColoredConsoleSink : ILogEventSink
    {
        private readonly ConsoleColor defaultForeground = ConsoleColor.White;
        private readonly ConsoleColor defaultBackground = ConsoleColor.Black;

        private readonly ITextFormatter formatter;

        public ColoredConsoleSink(ITextFormatter formatter)
        {
            this.formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            Console.BackgroundColor = ConsoleColor.Black;

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                }
                case LogEventLevel.Information:
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                case LogEventLevel.Warning:
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                }
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                {
                    Console.ForegroundColor = ConsoleColor.Red; break;
                }
                default: throw new NotSupportedException($"{logEvent.Level} is not a supported {typeof(LogEventLevel).Name}");
            }

            formatter.Format(logEvent, Console.Out);
            Console.Out.Flush();

            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
        }
    }
}
