﻿using System;

namespace DiscordBot.InfiltratorGame
{
    [Serializable]
    public class Stat
    {
        public string Name { get; set; }

        public int Value { get; set; }
        public int Max { get; set; }

        public Stat(string name, int max)
        {
            Name = name;

            Value = max;
            Max = max;
        }
    }
}
