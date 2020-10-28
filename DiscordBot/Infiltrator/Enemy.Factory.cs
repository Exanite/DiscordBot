﻿using System;
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

            private readonly EmbedHelper embedHelper;
            private readonly Random random;

            public Factory(EmbedHelper embedHelper, Random random)
            {
                this.embedHelper = embedHelper;
                this.random = random;
            }

            public Enemy Create(InfiltratorGame game)
            {
                var enemy = new Enemy(embedHelper, random);
                string name = GetRandomName();
                int health = random.Next(8, 12);
                int credits = random.Next(8, 12);

                enemy.Construct(name, health, credits);

                return enemy;
            }

            public string GetRandomName()
            {
                int prefixIndex = random.Next(0, PrefixNames.Count);
                int suffixIndex = random.Next(0, SuffixNames.Count);

                int nameIndex = random.Next(0, EnemyNames.Count);

                return $"{PrefixNames[prefixIndex]} {EnemyNames[nameIndex]} {SuffixNames[suffixIndex]}";
            }
        }
    }
}
