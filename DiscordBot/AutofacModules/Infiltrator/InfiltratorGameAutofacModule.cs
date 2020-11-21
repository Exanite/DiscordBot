using Autofac;
using DiscordBot.InfiltratorGame;

namespace DiscordBot.AutofacModules.Infiltrator
{
    public class InfiltratorGameAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameManager>().SingleInstance();
            builder.RegisterType<PlayerManager>().SingleInstance();
            builder.RegisterType<SaveManager>().SingleInstance();

            builder.RegisterType<Game.Factory>().SingleInstance();
            builder.RegisterType<Player.Factory>().SingleInstance();
            builder.RegisterType<Enemy.Factory>().SingleInstance();
        }
    }
}
