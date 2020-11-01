using Serilog;

namespace DiscordBot.Logging.Serilog
{
    public class SerilogLogAdapter<T> : SerilogLogAdapter, ILog<T>
    {
        public SerilogLogAdapter(ILogger inner) : base(inner.ForContext<T>()) { }
    }
}
