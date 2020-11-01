﻿using System;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame.Data
{
    public class GameData
    {
        [JsonConstructor]
        private GameData() { }

        public GameData(ulong guildId, ulong channelId, DateTimeOffset startTime)
        {
            GuildId = guildId;
            ChannelId = channelId;
            StartTime = startTime;
        }

        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public EnemyData EnemyData { get; set; }
    }
}
