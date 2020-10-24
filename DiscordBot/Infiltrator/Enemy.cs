namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public string name;
        public Stat health;

        public Enemy(string name, int health)
        {
            this.name = name;
            this.health = new Stat("Health", health, health);
        }
    }
}
