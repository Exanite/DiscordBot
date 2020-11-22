﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Extensions;
using DiscordBot.Core.Services;
using DiscordBot.InfiltratorGame.Data;

namespace DiscordBot.InfiltratorGame
{
    public partial class Game
    {
        public static readonly IEmote AttackEmote = new Emoji("⚔️");

        private IUserMessage enemyMessage;
        private IMessageChannel channel;

        private Enemy enemy;

        private readonly PlayerManager playerManager;
        private readonly EmbedHelper embedHelper;
        private readonly Enemy.Factory enemyFactory;

        public Game(PlayerManager playerManager, EmbedHelper embedHelper, Enemy.Factory enemyFactory, IMessageChannel channel, GameData data)
        {
            if (channel.Id != data.ChannelId)
            {
                throw new ArgumentException("channel.Id must match data.ChannelId.");
            }

            this.playerManager = playerManager;
            this.embedHelper = embedHelper;
            this.enemyFactory = enemyFactory;

            this.channel = channel;

            Data = data;

            if (data.EnemyData != null)
            {
                Enemy = enemyFactory.Create(data.EnemyData);
            }
        }

        public GameData Data { get; set; }

        public Enemy Enemy
        {
            get
            {
                return enemy;
            }

            set
            {
                enemy = value;
                Data.EnemyData = enemy.Data;
            }
        }

        public async Task CreateAndShowNewEnemy() // todo split into different methods
        {
            if (Enemy != null)
            {
                enemyMessage.RemoveAllReactionsAsync().Forget();
            }

            Enemy = enemyFactory.Create();

            enemyMessage = await channel.SendMessageAsync(embed: Enemy.ToEmbed());
            await enemyMessage.AddReactionAsync(AttackEmote);
        }

        public Embed ToEmbed()
        {
            return embedHelper.CreateBuilder("[Infiltrator Game Info]", "Shows information about the current game.")
                .AddField("Running in", channel.Name)
                .AddField("Started at", Data.StartTime)
                .AddField("Player count", playerManager.PlayersById.Count)
                .Build();
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (this.channel.Id != channel.Id
                || cacheable.Id != enemyMessage.Id
                || reaction.User.GetValueOrDefault().IsBot)
            {
                return;
            }

            if (reaction.Emote.Name == AttackEmote.Name)
            {
                var user = reaction.User.GetValueOrDefault();
                var player = playerManager.GetFor(user);

                Enemy.OnAttacked(player);

                await Task.WhenAll(
                    enemyMessage.ModifyAsync(x => x.Embed = Enemy.ToEmbed()),
                    enemyMessage.RemoveReactionAsync(reaction.Emote, reaction.User.GetValueOrDefault()));

                if (Enemy.Data.Health.Value <= 0)
                {
                    await CreateAndShowNewEnemy();
                }
            }
        }
    }
}
