using System;
using Discord;
using DiscordBot.InfiltratorGame.Data;
using DiscordBot.Services;

namespace DiscordBot.InfiltratorGame
{
    public partial class Game
    {
        public class Factory
        {
            private readonly PlayerManager playerManager;
            private readonly EmbedHelper embedHelper;
            private readonly Enemy.Factory enemyFactory;

            public Factory(PlayerManager playerManager, EmbedHelper embedHelper, Enemy.Factory enemyFactory)
            {
                this.embedHelper = embedHelper;
                this.enemyFactory = enemyFactory;
                this.playerManager = playerManager;
            }

            public Game Create(IGuild guild, IMessageChannel channel)
            {
                var gameData = new GameData(guild.Id, channel.Id, DateTimeOffset.Now);
                var game = new Game(playerManager, embedHelper, enemyFactory, channel, gameData);

                return game;
            }
        }
    }
}
