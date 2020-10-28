namespace DiscordBot.Infiltrator
{
    public class Stat
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Max { get; set; }

        public Stat(string name, int max) : this(name, max, max) { }

        public Stat(string name, int value, int max)
        {
            Name = name;
            Value = value;
            Max = max;
        }
    }
}
