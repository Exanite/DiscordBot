using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Extensions;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class InfiltratorGame
    {
        public static readonly IEmote attackEmote = new Emoji("⚔️");

        public List<Player> players = new List<Player>();

        public Enemy enemy;
        public IUserMessage enemyMessage;

        public int difficultyLevel = 0;
        public DateTimeOffset startTime;
        public IMessageChannel channel;

        private DiscordSocketClient Client { get; }
        private EmbedHelper EmbedHelper { get; }
        private Enemy.Factory EnemyFactory { get; }

        public InfiltratorGame(DiscordSocketClient client, EmbedHelper embedHelper, Enemy.Factory enemyFactory, IMessageChannel channel)
        {
            Client = client;
            EmbedHelper = embedHelper;
            EnemyFactory = enemyFactory;

            this.channel = channel;
            startTime = DateTimeOffset.Now;

            client.ReactionAdded += OnReactionAdded;
        }

        public async Task CreateAndShowNewEnemy() // todo split into different methods
        {
            if (enemy != null)
            {
                enemyMessage.RemoveAllReactionsAsync().Forget();
            }

            enemy = EnemyFactory.Create(this);

            enemyMessage = await channel.SendMessageAsync(embed: enemy.ToEmbed());
            await enemyMessage.AddReactionAsync(attackEmote);
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("Infiltrator Game Info", "Shows information about the current game.")
                .AddField("Running in", channel.Name)
                .AddField("Started at", startTime)
                .AddField("Player count", players.Count)
                .AddField("Difficulty level", difficultyLevel)
                .Build();
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (this.channel.Id != channel.Id
                || cacheable.Id != enemyMessage.Id
                || reaction.User.GetValueOrDefault().IsBot)
            {
                return;
            }

            if (reaction.Emote.Name == attackEmote.Name)
            {
                enemy.OnAttacked(this, null); // ! use Player reference instead of null

                await Task.WhenAll(
                    enemyMessage.ModifyAsync(x => x.Embed = enemy.ToEmbed()),
                    enemyMessage.RemoveReactionAsync(reaction.Emote, reaction.User.GetValueOrDefault()));

                if (enemy.health.value <= 0)
                {
                    await channel.SendMessageAsync($"{enemy.name} has been defeated by {reaction.User.GetValueOrDefault().Username}.");
                    await CreateAndShowNewEnemy();
                }
            }
        }
    }
}
