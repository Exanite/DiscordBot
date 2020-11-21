using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DiscordBot.Core.Logging;
using DiscordBot.Core.Services;

namespace DiscordBot
{
    public class Program
    {
        private IContainer container;
        private ILog log;
        private DiscordBotService bot;

        public static void Main()
        {
            var program = new Program();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                program.Exit();
            };

            try
            {
                program.MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                if (program.log != null)
                {
                    program.log.Fatal(e, "Unhandled exception.");
                }
                else // fallback if exception was caught during container build
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("\nUnhandled exception. Press any key to exit...");
                Console.ReadKey();
            }
        }

        public async Task MainAsync()
        {
            container = CreateContainer();

            log = container.Resolve<ILog<Program>>();

            container.Resolve<CommandHandler>();
            container.Resolve<DiscordLoggingService>();

            bot = container.Resolve<DiscordBotService>();

            await bot.Start();

            await Task.Delay(-1);
        }

        public void Exit()
        {
            if (bot != null)
            {
                bot.Stop().GetAwaiter().GetResult();
            }

            Environment.Exit(0);
        }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());

            return builder.Build();
        }
    }
}
