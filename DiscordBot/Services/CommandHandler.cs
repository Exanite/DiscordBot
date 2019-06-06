using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Configuration;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class CommandHandler
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly DiscordBotConfig config;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService commands, DiscordBotConfig config)
        {
            this.provider = provider;
            this.client = client;
            this.commands = commands;
            this.config = config;

            client.MessageReceived += MessageReceived;
            commands.CommandExecuted += CommandExecuted;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage) || message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(client, userMessage);
            int argPos = 0;

            if (userMessage.HasStringPrefix(config.Commands.Prefix, ref argPos) || userMessage.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var result = await commands.ExecuteAsync(context, argPos, provider);
            }
        }

        private async Task CommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess && config.Commands.ShowErrorMessages)
            {
                var reply = await SendErrorMessage(context, result);

                if (config.Commands.DeleteErrorMessages)
                {
                    await DeleteAfterTime(context.Message, config.Commands.ErrorMessageDeletionTime * 1000);
                    await DeleteAfterTime(reply, config.Commands.ErrorMessageDeletionTime * 1000);
                }
            }
        }

        private async Task<IMessage> SendErrorMessage(ICommandContext context, IResult result)
        {
            return await context.Channel.SendMessageAsync($"Unable to process command. Reason: {result.ErrorReason}");
        }

        private async Task DeleteAfterTime(IMessage message, int millisecondsDelay)
        {
            try
            {
                await Task.Delay(millisecondsDelay);

                await message.DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
