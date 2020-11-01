using System;
using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Services;
using DiscordBot.InfiltratorGame.Data;

namespace DiscordBot.InfiltratorGame
{
    public partial class Game
    {
        public class Factory
        {
            private readonly PlayerManager playerManager;
            private readonly EmbedHelper embedHelper;
            private readonly Enemy.Factory enemyFactory;
            private readonly DiscordSocketClient client;

            public Factory(PlayerManager playerManager, EmbedHelper embedHelper, Enemy.Factory enemyFactory, DiscordSocketClient client)
            {
                this.embedHelper = embedHelper;
                this.enemyFactory = enemyFactory;
                this.playerManager = playerManager;
                this.client = client;
            }

            public Game Create(GameData data)
            {
                var guild = client.GetGuild(data.GuildId);
                var channel = guild.GetTextChannel(data.ChannelId);

                return Create(channel, data);
            }

            public Game Create(IGuild guild, IMessageChannel channel)
            {
                var data = new GameData(guild.Id, channel.Id, DateTimeOffset.Now);

                return Create(channel, data);
            }

            public Game Create(IMessageChannel channel, GameData data)
            {
                return new Game(playerManager, embedHelper, enemyFactory, channel, data);
            }
        }
    }
}
