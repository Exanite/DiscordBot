using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.AutofacModules.Core
{
    public class ProgramAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Populate(new ServiceCollection());
        }
    }
}
