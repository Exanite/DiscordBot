using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame.Models
{
    public class PlayerData
    {
        [JsonConstructor]
        private PlayerData() { }

        public PlayerData(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; set; }

        public int Credits { get; set; }
    }
}
