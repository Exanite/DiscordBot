using System;
using System.Collections.Generic;
using DiscordBot.Services;

namespace DiscordBot.Infiltrator
{
    public partial class Enemy
    {
        public class Factory
        {
            public static List<string> PrefixNames = new List<string>() // todo move this to a config file
            {
                "Blurred",
                "Calculating",
                "Chosen",
                "Corrupted",
                "Cruel",
                "Dangerous",
                "Deadly",
                "Foreboding",
                "Hostile",
                "Lethal",
                "Malicious",
                "Merciless",
                "Nihilist's",
                "Planning",
                "Precise",
                "Resilient",
                "Skilled",
                "Spiteful",
                "Swift",
                "The Infiltrator's",
                "Unreal",
                "Vengeful",
                "Vicious",
            };

            public static List<string> SuffixNames = new List<string>()
            {
                "of Accuracy",
                "of Anger",
                "of Annihilation",
                "of Cunning",
                "of Destruction",
                "of Devouring",
                "of Doom",
                "of Efficiency",
                "of Evasion",
                "of Evil",
                "of Exile",
                "of Ferocity",
                "of Finesse",
                "of Intent",
                "of Legerdemain",
                "of Plunder",
                "of Power",
                "of Precision",
                "of Rage",
                "of Raiding",
                "of Ruin",
                "of Sortilege",
                "of the Campaign",
                "of the Elder",
                "of the Path",
            };

            public static List<string> EnemyNames = new List<string>()
            {
                "Agent",
                "Assassin",
                "Bug",
                "Executioner",
                "Exploiter",
                "Glitch",
                "Hacker",
                "Hitman",
                "Mercenary",
                "Operative",
                "Rogue",
                "Secret Agent",
                "Shade",
                "Shroud",
                "Slayer",
                "Spy",
                "Terminator",
                "Virus",
            };

            public Factory(EmbedHelper embedHelper, Random random)
            {
                EmbedHelper = embedHelper;
                Random = random;
            }

            private EmbedHelper EmbedHelper { get; }
            private Random Random { get; }

            public Enemy Create(InfiltratorGame game)
            {
                int nameIndex = Random.Next(0, EnemyNames.Count);

                var enemy = new Enemy(EmbedHelper, Random);
                enemy.Construct(GetRandomName(), 10);

                return enemy;
            }

            public string GetRandomName()
            {
                int prefixIndex = Random.Next(0, PrefixNames.Count);
                int suffixIndex = Random.Next(0, SuffixNames.Count);

                int nameIndex = Random.Next(0, EnemyNames.Count);

                return $"{PrefixNames[prefixIndex]} {EnemyNames[nameIndex]} {SuffixNames[suffixIndex]}";
            }
        }
    }
}
