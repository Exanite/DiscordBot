using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Infiltrator
{
    [Group("RPG")]
    [Summary("Commands for the Infiltrator text-based RPG.")]
    public class InfiltratorGameModule : ModuleBase<SocketCommandContext>
    {
        private readonly InfiltratorGameManager gameManager;

        public InfiltratorGameModule(InfiltratorGameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        [Command("StartGame")]
        [Summary("Starts a new game of Infiltrator in the current channel.")]
        public async Task StartGame()
        {
            await Context.Channel.SendMessageAsync("Starting a new game of Infiltrator.");

            gameManager.currentGame = new InfiltratorGame(Context);

            await gameManager.currentGame.Start();
        }

        [Command("Current")]
        [Summary("Shows information about the current game.")]
        public async Task GetCurrent()
        {
            var game = gameManager.currentGame;

            if (game != null)
            {
                await Context.Channel.SendMessageAsync(embed: game.BuildGameInfoEmbed());
            }
            else
            {
                await Context.Channel.SendMessageAsync("No games active.");
            }
        }
    }

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

    public class Enemy
    {
        public string name;
        public Stat health;

        public Enemy(string name, int health)
        {
            this.name = name;
            this.health = new Stat("Health", health, health);
        }
    }

    public class Player
    {

    }

    public class Stat
    {
        public string name;
        public int value;
        public int max;

        public Stat(string name, int value, int max)
        {
            this.name = name;
            this.value = value;
            this.max = max;
        }
    }
}
