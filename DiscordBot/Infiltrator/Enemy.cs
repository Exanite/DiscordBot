using System;
using Discord;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public static readonly string SpawnActionMessage = "A wild Infiltrator has appeared!";

        public string name;
        public Stat health;

        public string lastActionMessage;

        public Enemy(EmbedHelper embedHelper, Random random)
        {
            EmbedHelper = embedHelper;
            Random = random;

            lastActionMessage = SpawnActionMessage;
        }

        public void Construct(string name, int health)
        {
            this.name = name;
            this.health = new Stat("Health", health, health);
        }

        public EmbedHelper EmbedHelper { get; }
        public Random Random { get; }

        public void OnAttacked(InfiltratorGame game, Player player)
        {
            int damage = Random.Next(1, 5);

            health.value -= damage;

            lastActionMessage = $"Attacked by {player} for {damage} damage ";
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("Infiltrator Battle", lastActionMessage)
                .AddField("Name", name)
                .AddField(health.name, $"{health.value}/{health.max}")
                .Build();
        }
    }
}
