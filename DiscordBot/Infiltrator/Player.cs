using Discord;

namespace DiscordBot.Infiltrator
{
    public class Player
    {
        public Player(IUser user)
        {
            Id = user.Id;
            Username = user.Username;
            Discriminator = user.Discriminator;
            Mention = user.Mention;
        }

        public ulong Id { get; }
        public string Username { get; }
        public string Discriminator { get; }
        public string Mention { get; }

        public string FullUsername => $"{Username}:{Discriminator}";

        public override string ToString()
        {
            return FullUsername;
        }
    }
}
