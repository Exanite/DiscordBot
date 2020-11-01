using Autofac;
using Autofac.Extensions.DependencyInjection;
using DiscordBot.Core.Services;
using DiscordBot.InfiltratorGame;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.AutofacModules
{
    public class ProgramAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Populate(new ServiceCollection());
            builder.Register(_ => typeof(DiscordBotService).Assembly).SingleInstance(); // todo find better way of registering assemblies
            builder.Register(_ => typeof(GameManager).Assembly).SingleInstance();
        }
    }
}
