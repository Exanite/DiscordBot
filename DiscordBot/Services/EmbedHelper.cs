using Discord;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    public class EmbedHelper
    {
        public static readonly Color DefaultColor = Color.Gold;

        public EmbedHelper(DiscordSocketClient client)
        {
            Client = client;
        }

        public DiscordSocketClient Client { get; }

        public EmbedBuilder CreateBuilder(string name, string description)
        {
            return CreateBuilder(name, description, DefaultColor);
        }

        public EmbedBuilder CreateBuilder(string name, string description, Color color)
        {
            return new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = Client.CurrentUser.GetAvatarUrl(),
                    Name = name,
                })
                .WithDescription(description)
                .WithColor(color)
                .WithCurrentTimestamp();
        }
    }
}
