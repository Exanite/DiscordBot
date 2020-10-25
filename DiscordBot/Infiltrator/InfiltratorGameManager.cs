using Discord;
using Discord.WebSocket;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGameManager
    {
        public InfiltratorGameManager(InfiltratorGame.Factory gameFactory, DiscordSocketClient client)
        {
            GameFactory = gameFactory;
            Client = client;
        }

        public InfiltratorGame CurrentGame { get; private set; }

        private InfiltratorGame.Factory GameFactory { get; }
        private DiscordSocketClient Client { get; }

        public InfiltratorGame CreateGame(ITextChannel channel)
        {
            if (CurrentGame != null)
            {
                Client.ReactionAdded -= CurrentGame.OnReactionAdded;
            }

            CurrentGame = GameFactory.CreateGame(channel);

            Client.ReactionAdded += CurrentGame.OnReactionAdded;

            return CurrentGame;
        }
    }
}
