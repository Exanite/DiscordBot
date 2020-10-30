using System.Collections.Generic;
using Discord;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerManager
    {
        private readonly Dictionary<ulong, Player> PlayerDictionary = new Dictionary<ulong, Player>();

        [JsonProperty]
        public IReadOnlyDictionary<ulong, Player> PlayersById => PlayerDictionary;

        public Player GetFor(IUser user)
        {
            if (PlayerDictionary.TryGetValue(user.Id, out Player player))
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
            PlayerDictionary.Add(player.Id, player);

            return player;
        }
    }
}
