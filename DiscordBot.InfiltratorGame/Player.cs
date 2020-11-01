using System;
using Discord;
using DiscordBot.InfiltratorGame.Models;

namespace DiscordBot.InfiltratorGame
{
    public partial class Player
    {
        public Player(IUser user, PlayerData data)
        {
            if (user.Id != data.Id)
            {
                throw new ArgumentException("user.Id must match data.Id.");
            }

            User = user;

            Data = data;
        }

        public IUser User { get; }

        public PlayerData Data { get; set; }

        public Embed ToEmbed()
        {
            var builder = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = User.GetAvatarUrl(),
                    Name = $"[{User.Username}]",
                })
                .WithColor(Color.Blue)
                .WithCurrentTimestamp();

            builder.AddField("Credits", Data.Credits);

            return builder.Build();
        }
    }
}
