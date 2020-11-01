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

        public string SaveToJson()
        {
            var playerDataCollection = PlayerDictionary.Select(x => x.Value.Data).ToList();

            return JsonConvert.SerializeObject(playerDataCollection, Formatting.Indented);
        }

        public void LoadFromJson(string json)
        {
            PlayerDictionary.Clear();

            var playerDataCollection = JsonConvert.DeserializeObject<List<PlayerData>>(json);

            foreach (var playerData in playerDataCollection)
            {
                //var player = playerFactory.Create(playerData);

                //PlayerDictionary[player.Data.Id] = player;
            }
        }

        private Player CreateFor(IUser user)
        {
            var playerData = new PlayerData(user.Id);
            var player = new Player(user, playerData);

            PlayerDictionary.Add(player.User.Id, player);

            return player;
        }
    }
}
