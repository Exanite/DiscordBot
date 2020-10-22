using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Infiltrator
{

    public class InfiltratorGame
    {
        public List<Player> players = new List<Player>();
        public Enemy enemy;
        public IUserMessage enemyMessage;
        public int difficultyLevel = 0;

        public readonly DateTimeOffset startTime;
        public readonly IMessageChannel channel;
        public readonly ICommandContext context;

        public InfiltratorGame(ICommandContext context)
        {
            this.context = context; // remove later on
            this.channel = context.Channel;
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
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = context.Client.CurrentUser.GetAvatarUrl(),
                    Name = $"Infiltrator Battle",
                })
                .WithDescription("A wild Infiltrator has appeared!")
                .WithColor(Color.Gold)
                .WithCurrentTimestamp();

            embed
                .AddField("Name", enemy.name)
                .AddField(enemy.health.name, $"{enemy.health.value}/{enemy.health.max}");

            return embed.Build();
        }

        public Embed BuildGameInfoEmbed()
        {
            var embed = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = context.Client.CurrentUser.GetAvatarUrl(),
                    Name = $"Infiltrator Game Info",
                })
                .WithDescription("Shows information about the current game.")
                .WithColor(Color.Gold)
                .WithCurrentTimestamp();

            embed
                .AddField("Running in channel", channel.Name)
                .AddField("Started at", startTime)
                .AddField("Player count", players.Count)
                .AddField("Difficulty level", difficultyLevel);

            return embed.Build();
        }
    }
}
