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
        }

        public Game CurrentGame { get; private set; }

        public Game CreateGame(ITextChannel channel)
        {
            if (CurrentGame != null)
            {
                client.ReactionAdded -= CurrentGame.OnReactionAdded;
            }

            CurrentGame = gameFactory.CreateGame(channel);

            client.ReactionAdded += CurrentGame.OnReactionAdded;

            return CurrentGame;
        }
    }
}
