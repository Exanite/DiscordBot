using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGame
    {
        public List<Player> players = new List<Player>();
        public Enemy enemy;
        public IUserMessage enemyMessage;
        public int difficultyLevel = 0;

        public DateTimeOffset startTime;
        public IMessageChannel channel;

        private readonly IDiscordClient client;

        public InfiltratorGame(IDiscordClient client, IMessageChannel channel)
        {
            this.client = client;
            this.channel = channel;
            this.startTime = DateTimeOffset.Now;
        }

        public async Task Start()
        {
            enemy = new Enemy("Assassin", 10);

            enemyMessage = await channel.SendMessageAsync(embed: BuildEnemyEmbed());
            await enemyMessage.AddReactionAsync(new Emoji("⚔️"));
            await enemyMessage.AddReactionAsync(new Emoji("📋"));
            await enemyMessage.AddReactionAsync(new Emoji("👜"));
            await enemyMessage.AddReactionAsync(new Emoji("💨"));
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
    }
}
