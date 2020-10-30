using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Extensions;
using DiscordBot.Services;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Game
    {
        public static readonly IEmote AttackEmote = new Emoji("⚔️");

        private IUserMessage enemyMessage;
        private IMessageChannel channel;

        private readonly PlayerManager playerManager;
        private readonly EmbedHelper embedHelper;
        private readonly Enemy.Factory enemyFactory;

        public Game(PlayerManager playerManager, EmbedHelper embedHelper, Enemy.Factory enemyFactory, IMessageChannel channel)
        {
            this.playerManager = playerManager;
            this.embedHelper = embedHelper;
            this.enemyFactory = enemyFactory;

            this.channel = channel;

            StartTime = DateTimeOffset.Now;
        }

        [JsonProperty]
        public Enemy Enemy;

        [JsonProperty]
        public int DifficultyLevel = 0;

        [JsonProperty]
        public DateTimeOffset StartTime;

        public async Task CreateAndShowNewEnemy() // todo split into different methods
        {
            if (Enemy != null)
            {
                enemyMessage.RemoveAllReactionsAsync().Forget();
            }

            Enemy = enemyFactory.Create(this);

            enemyMessage = await channel.SendMessageAsync(embed: Enemy.ToEmbed());
            await enemyMessage.AddReactionAsync(AttackEmote);
        }

        public Embed ToEmbed()
        {
            return embedHelper.CreateBuilder("[Infiltrator Game Info]", "Shows information about the current game.")
                .AddField("Running in", channel.Name)
                .AddField("Started at", StartTime)
                .AddField("Player count", playerManager.PlayersById.Count)
                .AddField("Difficulty level", DifficultyLevel)
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

                if (Enemy.Health.Value <= 0)
                {
                    await CreateAndShowNewEnemy();
                }
            }
        }
    }
}
