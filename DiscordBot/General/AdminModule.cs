using Discord;
using Discord.Commands;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.General
{
    [Group("Admin")]
    [Summary("Commands used for administering the state of a Discord guild.")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("Kick")]
        [Summary("Kicks a specified user.")]
        [RequireBotPermission(GuildPermission.KickMembers), RequireUserPermission(GuildPermission.KickMembers), RequireContext(ContextType.Guild)]
        public async Task Kick(IGuildUser user, [Remainder] string reason = "No reason was provided.")
        {
            await user.KickAsync(reason);
            await Context.Channel.SendMessageAsync($"Successfully kicked user '{user.Username}:{user.Discriminator}' with reason: '{reason}'.");
        }

        [Command("Ban")]
        [Summary("Bans a specified user.")]
        [RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers), RequireContext(ContextType.Guild)]
        public async Task Ban(IGuildUser user, [Remainder] string reason = "No reason was provided.")
        {
            await user.BanAsync(reason: reason);
            await Context.Channel.SendMessageAsync($"Successfully banned user '{user.Username}:{user.Discriminator}' with reason: '{reason}'.");
        }

        [Command("Unban")]
        [Summary("Unbans a specified user.")]
        [RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers), RequireContext(ContextType.Guild)]
        public async Task Unban(ulong userID)
        {
            var user = Context.Client.GetUser(userID);

            if (user == null)
            {
                throw new Exception("User cannot be found.");
            }

            try
            {
                await Context.Guild.RemoveBanAsync(user);
                await Context.Channel.SendMessageAsync($"Successfully unbanned user '{user.Username}:{user.Discriminator}'.");
            }
            catch (HttpException e)
            {
                throw new Exception("User is currently not banned", e);
            }
        }

        [Command("DeleteMessages")]
        [Summary("Deletes a number of messages from the current channel.")]
        [RequireBotPermission(GuildPermission.ManageMessages), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task ClearMessages(int number, IMessageChannel channel = null)
        {
            if (channel == null)
            {
                channel = Context.Channel;
            }

            var deleteTasks = new List<Task>(number);
            var messages = await channel.GetMessagesAsync(Context.Message, Direction.Before, number).FlattenAsync();
            foreach (var message in messages)
            {
                var task = message.DeleteAsync();
                deleteTasks.Add(task);
            }

            await Task.WhenAll(deleteTasks);

            await Context.Channel.SendMessageAsync($"Successfully deleted {number} messages from '{channel.Name}'");
        }
    }
}
