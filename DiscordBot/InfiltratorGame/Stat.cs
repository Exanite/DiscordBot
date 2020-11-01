namespace DiscordBot.InfiltratorGame
{
    public class Stat
    {
        public Stat(string name, int max)
        {
            Name = name;

            Value = max;
            Max = max;
        }

        public string Name { get; set; }

        public int Value { get; set; }
        public int Max { get; set; }
    }
}
