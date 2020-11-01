using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame.Data
{
    public class EnemyData
    {
        public const string DefaultActionMessage = "A wild Infiltrator has appeared!";

        [JsonConstructor]
        private EnemyData() { }

        public EnemyData(string name, int health, int credits)
        {
            Name = name;
            Health = new Stat("Health", health);
            Credits = credits;

            LastActionMessage = DefaultActionMessage;
        }

        public string Name { get; set; }

        public Stat Health { get; set; }

        public int Credits { get; set; }

        public string LastActionMessage { get; set; }
    }
}
