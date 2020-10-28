using System;
using Discord;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public static readonly string SpawnActionMessage = "A wild Infiltrator has appeared!";

        public string name;
        public int credits;

        public Stat health;

        public string lastActionMessage;

        public event Action<Player, Enemy, int> Attacked;
        public event Action<Player, Enemy> Killed;

        public Enemy(EmbedHelper embedHelper, Random random)
        {
            EmbedHelper = embedHelper;
            Random = random;

            lastActionMessage = SpawnActionMessage;
        }

        public void Construct(string name, int health, int credits)
        {
            this.name = name;
            this.health = new Stat("Health", health);
            this.credits = credits;
        }

        public EmbedHelper EmbedHelper { get; }
        public Random Random { get; }

        public void OnAttacked(Player player)
        {
            int damage = Random.Next(1, 5);

            health.value -= damage;

            lastActionMessage = $"Attacked by {player} for {damage} damage ";

            Attacked?.Invoke(player, this, damage);

            if (health.value < 0)
            {
                Killed?.Invoke(player, this);
            }
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("[Infiltrator Battle]", lastActionMessage)
                .AddField("Name", name)
                .AddField(health.name, $"{health.value}/{health.max}")
                .AddField("Credits", credits)
                .Build();
        }
    }
}
