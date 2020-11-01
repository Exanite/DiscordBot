using System.Collections.Generic;
using System.Linq;
using Discord;
using DiscordBot.InfiltratorGame.Models;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    public class PlayerManager
    {
        private readonly Dictionary<ulong, Player> PlayerDictionary = new Dictionary<ulong, Player>();

        private readonly Player.Factory playerFactory;

        public PlayerManager(Player.Factory playerFactory)
        {
            this.playerFactory = playerFactory;
        }

        public IReadOnlyDictionary<ulong, Player> PlayersById => PlayerDictionary;

        public Player GetFor(IUser user)
        {
            if (!PlayerDictionary.TryGetValue(user.Id, out Player player))
            {
                player = playerFactory.Create(user);

                PlayerDictionary.Add(player.User.Id, player);
            }

            return player;
        }

        public string SaveToJson()
        {
            List<PlayerData> playerDataCollection = PlayerDictionary.Select(x => x.Value.Data).ToList();

            return JsonConvert.SerializeObject(playerDataCollection, Formatting.Indented);
        }

        public void LoadFromJson(string json)
        {
            PlayerDictionary.Clear();

            List<PlayerData> playerDataCollection = JsonConvert.DeserializeObject<List<PlayerData>>(json);

            foreach (var playerData in playerDataCollection)
            {
                var player = playerFactory.Create(playerData);

                PlayerDictionary[player.Data.Id] = player;
            }
        }
    }
}
