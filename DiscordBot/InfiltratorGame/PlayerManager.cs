using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    public class PlayerManager
    {
        [JsonProperty]
        public Dictionary<ulong, Player> PlayersById = new Dictionary<ulong, Player>();
    }
}
