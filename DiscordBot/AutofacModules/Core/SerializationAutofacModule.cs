using Autofac;
using DiscordBot.Core.Serialization;

namespace DiscordBot.AutofacModules.Core
{
    public class SerializationAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SerializationService>().SingleInstance();
            builder.Register(x => x.Resolve<SerializationService>().Serializer).SingleInstance();
        }
    }
}
