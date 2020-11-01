﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.InfiltratorGame.Data;
using Newtonsoft.Json;

namespace DiscordBot.InfiltratorGame
{
    public class GameManager
    {
        private readonly Game.Factory gameFactory;

        public GameManager(Game.Factory gameFactory, DiscordSocketClient client)
        {
            this.gameFactory = gameFactory;

            client.ReactionAdded += OnReactionAdded;
        }

        public Dictionary<ulong, Game> Games { get; } = new Dictionary<ulong, Game>();

        public Game CreateGame(IGuild guild, IMessageChannel channel)
        {
            var game = gameFactory.Create(guild, channel);
            Games[channel.Id] = game;

            return game;
        }

        public string SaveToJson()
        {
            List<GameData> gameDataCollection = Games.Select(x => x.Value.Data).ToList();

            return JsonConvert.SerializeObject(gameDataCollection, Formatting.Indented);
        }

        public void LoadFromJson(string json)
        {
            Games.Clear();

            List<GameData> gameDataCollection = JsonConvert.DeserializeObject<List<GameData>>(json);

            foreach (var gameData in gameDataCollection)
            {
                var game = gameFactory.Create(gameData);

                Games[game.Data.ChannelId] = game;
            }
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (Games.TryGetValue(channel.Id, out Game game))
            {
                await game.OnReactionAdded(cacheable, channel, reaction);
            }
        }
    }
}
