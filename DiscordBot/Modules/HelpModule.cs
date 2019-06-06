using Discord;
using Discord.Commands;
using DiscordBot.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    [Name("Help")]
    [Summary("Commands used to show how to use this bot.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;

        public HelpModule(CommandService commands, DiscordBotConfig config)
        {
            this.commands = commands;
            this.config = config;
        }

        [Command("Help")]
        [Summary("Shows all modules and their commands.")]
        public async Task Help()
        {
            await ListAllModules();
        }

        private async Task ListAllModules()
        {
            foreach (var module in commands.Modules)
            {
                await ListModule(module);
            }
        }

        private async Task ListModule(ModuleInfo module)
        {
            var embed = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    IconUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                    Name = $"[{GetModuleName(module)} Module]",
                },
                Description = module.Summary,
            };

            foreach (var command in module.Commands)
            {
                string title = string.Empty;

                title += $"{config.Commands.Prefix}";

                foreach (var group in GetCommandGroups(command))
                {
                    title += $"{group} ";
                }

                title += $"{command.Name}";

                foreach (var parameter in command.Parameters)
                {
                    string prefix = parameter.IsOptional ? "[" : "<";
                    string name = parameter.Name;
                    string suffix = parameter.IsOptional ? "]" : ">";

                    title += $" {prefix}{name}{suffix}";
                }

                string description = command.Summary;
                embed.AddField(title, description);
            }

            embed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        private string GetModuleName(ModuleInfo module)
        {
            string result = string.Empty;

            if (module.Parent != null)
            {
                result += $"{GetModuleName(module.Parent)}.";
            }

            result += module.Name;

            return result;
        }

        private List<string> GetCommandGroups(CommandInfo command)
        {
            var result = new List<string>();
            var module = command.Module;

            do
            {
                if (!string.IsNullOrWhiteSpace(module.Group))
                {
                    result.Insert(0, module.Group);
                }

                module = module.Parent;

            } while (module != null);

            return result;
        }
    }
}
