using System;
using Discord;
using DiscordBot.InfiltratorGame.Data;
using DiscordBot.Services;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Enemy
    {
        private readonly EmbedHelper EmbedHelper;
        private readonly Random Random;

        public Enemy(EmbedHelper embedHelper, Random random, EnemyData data)
        {
            EmbedHelper = embedHelper;
            Random = random;

            Data = data;
        }

        public event Action<Player, Enemy, int> Attacked;
        public event Action<Player, Enemy> Killed;

        public EnemyData Data { get; }

        public void OnAttacked(Player player)
        {
            int damage = Random.Next(1, 5);

            Data.Health.Value -= damage;

            Data.LastActionMessage = $"Attacked by {player.User.Mention} for {damage} damage.";
            Attacked?.Invoke(player, this, damage);

            if (Data.Health.Value < 0)
            {
                player.Data.Credits += Data.Credits;

                Data.LastActionMessage = $"Killed by {player.User.Mention}.\nThey recieved {Data.Credits} credits!";
                Killed?.Invoke(player, this);
            }
        }

        public Embed ToEmbed()
        {
            return EmbedHelper.CreateBuilder("[Infiltrator Battle]", Data.LastActionMessage)
                .AddField("Name", Data.Name)
                .AddField(Data.Health.Name, $"{Data.Health.Value}/{Data.Health.Max}")
                .AddField("Credits", Data.Credits)
                .Build();
        }
    }
}
