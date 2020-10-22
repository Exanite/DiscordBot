using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Extensions;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGame
    {
        public static readonly IEmote attackEmote = new Emoji("⚔️");

        public List<Player> players = new List<Player>();
        public Enemy enemy;
        public IUserMessage enemyMessage;
        public int difficultyLevel = 0;

        public DateTimeOffset startTime;
        public IMessageChannel channel;

        private readonly DiscordSocketClient client;
        private readonly Random random;

        public InfiltratorGame(DiscordSocketClient client, IMessageChannel channel)
        {
            this.client = client;
            this.channel = channel;
            this.startTime = DateTimeOffset.Now;

            random = new Random(startTime.Millisecond);

            client.ReactionAdded += OnReactionAdded;
        }

        public async Task Start()
        {
            enemy = new Enemy("Assassin", 10);

            enemyMessage = await channel.SendMessageAsync(embed: BuildEnemyEmbed());

            enemyMessage.AddReactionAsync(attackEmote).Forget();
        }

        public Embed BuildEnemyEmbed()
        {
            return CreateBaseEmbedBuilder("Infiltrator Battle", "A wild Infiltrator has appeared!")
                .AddField("Name", enemy.name)
                .AddField(enemy.health.name, $"{enemy.health.value}/{enemy.health.max}")
                .Build();
        }

        public Embed BuildGameInfoEmbed()
        {
            return CreateBaseEmbedBuilder("Infiltrator Game Info", "Shows information about the current game.")
                .AddField("Running in channel", channel.Name)
                .AddField("Started at", startTime)
                .AddField("Player count", players.Count)
                .AddField("Difficulty level", difficultyLevel)
                .Build();
        }

        public EmbedBuilder CreateBaseEmbedBuilder(string name, string description)
        {
            return new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = client.CurrentUser.GetAvatarUrl(),
                    Name = name,
                })
                .WithDescription(description)
                .WithColor(Color.Gold)
                .WithCurrentTimestamp();
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
                enemy.health.value -= random.Next(1, 5);

                enemyMessage.RemoveReactionAsync(reaction.Emote, reaction.User.GetValueOrDefault()).Forget();
                enemyMessage.ModifyAsync(x => x.Embed = BuildEnemyEmbed()).Forget();

                if (enemy.health.value <= 0)
                {
                    await channel.SendMessageAsync($"{enemy.name} has been defeated.");

                    await Start();
                }
            }
        }
    }
}
