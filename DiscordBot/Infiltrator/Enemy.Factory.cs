using System;
using System.Collections.Generic;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public class Factory
        {
            public static List<string> EnemyNames = new List<string>()
            {
                "Assassin",
                "Shade",
                "Hacker",
                "Shroud",
                "Spy",
                "Virus",
                "Slayer",
                "Executioner",
                "Hitman",
                "Mercenary",
                "Terminator",
                "Secret Agent",
                "Agent",
                "Operative",
                "Glitch",
                "Bug",
                "Exploiter",
            };

            private readonly Random random = new Random();

            public Enemy Create(InfiltratorGame game)
            {
                int nameIndex = random.Next(0, EnemyNames.Count);

                return new Enemy(EnemyNames[nameIndex], 10);
            }
        }
    }
}
