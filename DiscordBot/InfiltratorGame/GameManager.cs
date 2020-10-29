using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.InfiltratorGame
{
    public class GameManager
    {
        private readonly Game.Factory gameFactory;
        private readonly DiscordSocketClient client;

        public GameManager(Game.Factory gameFactory, DiscordSocketClient client)
        {
            this.gameFactory = gameFactory;
            this.client = client;

            client.ReactionAdded += OnReactionAdded;
        }

        public Dictionary<ulong, Game> Games { get; } = new Dictionary<ulong, Game>();

        public Game CreateGame(ITextChannel channel)
        {
            var game = gameFactory.CreateGame(channel);
            Games[channel.Id] = game;

            return game;
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
