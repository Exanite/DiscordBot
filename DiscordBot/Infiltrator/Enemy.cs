using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Infiltrator
{

    public class Enemy
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
