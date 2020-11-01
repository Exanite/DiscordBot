using Autofac;
using DiscordBot.Core.Logging;
using DiscordBot.Core.Logging.Serilog;
using DiscordBot.Core.Services;
using Serilog;

namespace DiscordBot.AutofacModules
{
    public class LoggingAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggingService>().SingleInstance();
            builder.Register(x => x.Resolve<LoggingService>().Logger).As<ILogger>().SingleInstance();

            builder.RegisterType<SerilogLogAdapter>().As<ILog>();
            builder.RegisterGeneric(typeof(SerilogLogAdapter<>)).As(typeof(ILog<>)).SingleInstance();
        }
    }
}
