using Discord;
using Discord.WebSocket;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGameManager
    {
        private readonly InfiltratorGame.Factory gameFactory;
        private readonly DiscordSocketClient client;

        public InfiltratorGameManager(InfiltratorGame.Factory gameFactory, DiscordSocketClient client)
        {
            this.gameFactory = gameFactory;
            this.client = client;
        }

        public InfiltratorGame CurrentGame { get; private set; }

        public InfiltratorGame CreateGame(ITextChannel channel)
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
