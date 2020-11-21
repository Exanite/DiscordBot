using System;
using Autofac;

namespace DiscordBot.AutofacModules.Core
{
    public class MiscellaneousAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Random>().SingleInstance();
        }
    }
}
