using Discord;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class InfiltratorGame
    {
        public class Factory
        {
            public Factory(EmbedHelper embedHelper, Enemy.Factory enemyFactory)
            {
                EmbedHelper = embedHelper;
                EnemyFactory = enemyFactory;
            }

            private EmbedHelper EmbedHelper { get; }
            private Enemy.Factory EnemyFactory { get; }

            public InfiltratorGame CreateGame(ITextChannel channel)
            {
                return new InfiltratorGame(EmbedHelper, EnemyFactory, channel);
            }
        }
    }
}
