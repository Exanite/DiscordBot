using Discord;
using Discord.WebSocket;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class InfiltratorGame
    {
        public class Factory
        {
            public Factory(DiscordSocketClient client, EmbedHelper embedHelper, Enemy.Factory enemyFactory)
            {
                Client = client;
                EmbedHelper = embedHelper;
                EnemyFactory = enemyFactory;
            }

            private DiscordSocketClient Client { get; }
            private EmbedHelper EmbedHelper { get; }
            private Enemy.Factory EnemyFactory { get; }

            public InfiltratorGame CreateGame(ITextChannel channel)
            {
                return new InfiltratorGame(Client, EmbedHelper, EnemyFactory, channel);
            }
        }
    }
}
