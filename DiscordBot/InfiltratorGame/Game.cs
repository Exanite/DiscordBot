using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Extensions;
using DiscordBot.Services;

namespace DiscordBot.InfiltratorGame
{
    public partial class Game
    {
        public static readonly IEmote attackEmote = new Emoji("⚔️");

        public Dictionary<ulong, Player> playersById = new Dictionary<ulong, Player>();

        public Enemy enemy;
        public IUserMessage enemyMessage;

        public int difficultyLevel = 0;
        public DateTimeOffset startTime;
        public IMessageChannel channel;

        private readonly EmbedHelper embedHelper;
        private readonly Enemy.Factory enemyFactory;

        public Game(EmbedHelper embedHelper, Enemy.Factory enemyFactory, IMessageChannel channel)
        {
            this.embedHelper = embedHelper;
            this.enemyFactory = enemyFactory;

            this.channel = channel;
            startTime = DateTimeOffset.Now;
        }

        public async Task CreateAndShowNewEnemy() // todo split into different methods
        {
            if (enemy != null)
            {
                enemyMessage.RemoveAllReactionsAsync().Forget();
            }

            enemy = enemyFactory.Create(this);

            enemyMessage = await channel.SendMessageAsync(embed: enemy.ToEmbed());
            await enemyMessage.AddReactionAsync(attackEmote);
        }

        public Embed ToEmbed()
        {
            return embedHelper.CreateBuilder("[Infiltrator Game Info]", "Shows information about the current game.")
                .AddField("Running in", channel.Name)
                .AddField("Started at", startTime)
                .AddField("Player count", playersById.Count)
                .AddField("Difficulty level", difficultyLevel)
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

            if (reaction.Emote.Name == attackEmote.Name)
            {
                if (!playersById.TryGetValue(reaction.UserId, out Player player))
                {
                    player = new Player(reaction.User.GetValueOrDefault());
                    playersById.Add(player.Id, player);
                }

                enemy.OnAttacked(player);

                await Task.WhenAll(
                    enemyMessage.ModifyAsync(x => x.Embed = enemy.ToEmbed()),
                    enemyMessage.RemoveReactionAsync(reaction.Emote, reaction.User.GetValueOrDefault()));

                if (enemy.Health.Value <= 0)
                {
                    await CreateAndShowNewEnemy();
                }
            }
        }
    }
}
