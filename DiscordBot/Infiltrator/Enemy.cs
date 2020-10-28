using System;
using Discord;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public const string SpawnActionMessage = "A wild Infiltrator has appeared!";

        private readonly EmbedHelper EmbedHelper;
        private readonly Random Random;

        public Enemy(EmbedHelper embedHelper, Random random)
        {
            EmbedHelper = embedHelper;
            Random = random;
        }

        public void Construct(string name, int health, int credits)
        {
            Name = name;
            Health = new Stat("Health", health);
            Credits = credits;

            LastActionMessage = SpawnActionMessage;
        }

        public event Action<Player, Enemy, int> Attacked;
        public event Action<Player, Enemy> Killed;

        public string Name { get; set; }
        public int Credits { get; set; }

        public Stat Health { get; set; }

        public string LastActionMessage { get; set; }

        public void OnAttacked(Player player)
        {
            int damage = Random.Next(1, 5);

            Health.Value -= damage;

            LastActionMessage = $"Attacked by {player} for {damage} damage ";

            Attacked?.Invoke(player, this, damage);

            if (Health.Value < 0)
            {
                Killed?.Invoke(player, this);
            }
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("[Infiltrator Battle]", LastActionMessage)
                .AddField("Name", Name)
                .AddField(Health.Name, $"{Health.Value}/{Health.Max}")
                .AddField("Credits", Credits)
                .Build();
        }
    }
}
