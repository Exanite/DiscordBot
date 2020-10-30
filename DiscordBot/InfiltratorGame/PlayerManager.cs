using System.Collections.Generic;
using Discord;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    public class PlayerManager
    {
        [JsonProperty]
        public Dictionary<ulong, Player> PlayersById { get; set; } = new Dictionary<ulong, Player>();

        public Player GetFor(IUser user)
        {
            if (PlayersById.TryGetValue(user.Id, out Player player))
            {
                return player;
            }
            else
            {
                return CreateFor(user);
            }
        }

        private Player CreateFor(IUser user)
        {
            var player = new Player(user);
            PlayersById.Add(player.Id, player);

            return player;
        }
    }
}
