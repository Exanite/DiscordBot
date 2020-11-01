using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot.General
{
    [Group("Bot")]
    [Summary("Commands for accessing or editing the state of this Discord bot.")]
    public class BotModule : ModuleBase<SocketCommandContext>
    {
        [Command("Ping")]
        [Summary("Shows the current ping of this bot to the Discord servers.")]
        public async Task Ping()
        {
            var message = await Context.Channel.SendMessageAsync("You dare ping me?");

            var time = message.CreatedAt.Subtract(Context.Message.Timestamp);

            await message.ModifyAsync((x) => x.Content = $"Pong! ({time.Milliseconds} ms)");
        }

        [Command("JoinVoice")]
        [Summary("Makes this bot join the specified voice channel.")]
        [RequireUserPermission(GuildPermission.MoveMembers), RequireContext(ContextType.Guild)]
        public async Task JoinVoice(IVoiceChannel voiceChannel)
        {
            await voiceChannel.ConnectAsync();
            await Context.Channel.SendMessageAsync($"Connected to '{voiceChannel.Name}'.");
        }

        [Command("LeaveVoice")]
        [Summary("Makes this bot leave the voice channel it is currently in, does nothing if not in one.")]
        [RequireUserPermission(GuildPermission.MoveMembers), RequireContext(ContextType.Guild)]
        public async Task LeaveVoice()
        {
            var voice = Context.Guild.CurrentUser.VoiceChannel;
            if (voice != null)
            {
                await voice.DisconnectAsync();
                await Context.Channel.SendMessageAsync($"Disconnected from '{voice.Name}'");
            }
            else
            {
                throw new Exception("Bot is currently not in a voice channel.");
            }
        }

        [Command("Say")]
        [Summary("Makes this bot send the specified message in the current channel.")]
        public async Task Say([Remainder]string message)
        {
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("Say"), Priority(1)]
        [Summary("Makes this bot send the specified message in a specific channel.")]
        [RequireBotPermission(GuildPermission.ManageMessages), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Say(IMessageChannel channel, [Remainder] string message)
        {
            await channel.SendMessageAsync(message);
        }
    }
}
