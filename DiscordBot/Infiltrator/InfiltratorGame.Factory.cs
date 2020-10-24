using Discord;
using Discord.WebSocket;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class InfiltratorGame
    {
        public class Factory
        {
            public Factory(DiscordSocketClient client, EmbedHelper embedHelper)
            {
                Client = client;
                EmbedHelper = embedHelper;
            }

            private DiscordSocketClient Client { get; }
            private EmbedHelper EmbedHelper { get; }

            public InfiltratorGame CreateGame(ITextChannel channel)
            {
                return new InfiltratorGame(Client, EmbedHelper, channel);
            }
        }
    }
}
