using Discord;
using DiscordBot.Services;

namespace DiscordBot.InfiltratorGame
{
    public partial class Game
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

            public Game CreateGame(ITextChannel channel)
            {
                return new Game(EmbedHelper, EnemyFactory, channel);
            }
        }
    }
}
