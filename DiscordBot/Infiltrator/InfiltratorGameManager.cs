using Discord;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGameManager
    {
        public InfiltratorGameManager(InfiltratorGame.Factory gameFactory)
        {
            GameFactory = gameFactory;
        }

        public InfiltratorGame CurrentGame { get; private set; }

        private InfiltratorGame.Factory GameFactory { get; }

        public InfiltratorGame CreateGame(ITextChannel channel)
        {
            CurrentGame = GameFactory.CreateGame(channel);

            return CurrentGame;
        }
    }
}
