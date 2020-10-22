namespace DiscordBot.Infiltrator
{
    public class Stat
    {
        public string name;
        public int value;
        public int max;

        public Stat(string name, int value, int max)
        {
            this.name = name;
            this.value = value;
            this.max = max;
        }
    }
}
