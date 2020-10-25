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
        }

        public ulong Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }

        public override string ToString()
        {
            return $"{Username}:{Discriminator}";
        }
    }
}
