using Discord;

namespace DiscordBot.Infiltrator
{
    public class Player
    {
        public Player(IUser user)
        {
            User = user;

            Id = user.Id;
            Username = user.Username;
            Discriminator = user.Discriminator;
            Mention = user.Mention;
        }

        public IUser User { get; } // ? not sure about using this here

        public ulong Id { get; }
        public string Username { get; }
        public string Discriminator { get; }
        public string Mention { get; }
        public string FullUsername => $"{Username}:{Discriminator}";

        public int Credits { get; set; }

        public override string ToString()
        {
            return FullUsername;
        }

        public Embed ToEmbed()
        {
            var builder = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    IconUrl = User.GetAvatarUrl(),
                    Name = Username,
                })
                .WithColor(Color.Blue)
                .WithCurrentTimestamp();

            builder.AddField("Credits", Credits);

            return builder.Build();
        }
    }
}
