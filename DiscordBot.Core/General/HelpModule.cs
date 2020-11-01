using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core.Configuration;
using DiscordBot.Core.Services;
using Color = Discord.Color;

namespace DiscordBot.Core.General
{
    [Name("Help")]
    [Summary("Commands used to show how to use this bot.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;
        private readonly EmbedHelper embedHelper;

        public HelpModule(CommandService commands, DiscordBotConfig config, EmbedHelper embedHelper)
        {
            this.commands = commands;
            this.config = config;
            this.embedHelper = embedHelper;
        }

        [Command("Help")]
        [Summary("Shows information about all modules.")]
        public async Task Help()
        {
            await ListAllModules();
        }

        [Command("Help")]
        [Summary("Shows information about the specified module and its commands.")]
        public async Task Help(string moduleName)
        {
            var module = commands.Modules.Where(x => string.Equals(x.Name, moduleName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (module != null)
            {
                await ListModule(module);
            }
            else
            {
                throw new Exception("Module not found.");
            }
        }

        private async Task ListAllModules()
        {
            var builder = embedHelper.CreateBuilder($"[All Modules]",
                $"Use '{config.Commands.Prefix}Help <moduleName>' to show information about a specific module.", Color.Green);

            foreach (var module in commands.Modules)
            {
                builder.AddField(module.Name, module.Summary);
            }

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task ListModule(ModuleInfo module)
        {
            var builder = embedHelper.CreateBuilder($"[{GetModuleName(module)} Module]", module.Summary, Color.Green);

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
                builder.AddField(title, description);
            }

            builder.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync(embed: builder.Build());
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
