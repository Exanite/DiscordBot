using System;
using Discord;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public string name;
        public Stat health;

        public Enemy(EmbedHelper embedHelper, Random random)
        {
            EmbedHelper = embedHelper;
            Random = random;
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
            health.value -= Random.Next(1, 5);
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("Infiltrator Battle", "A wild Infiltrator has appeared!")
                .AddField("Name", name)
                .AddField(health.name, $"{health.value}/{health.max}")
                .Build();
        }
    }
}
