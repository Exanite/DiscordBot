using Discord;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGameManager
    {
        public InfiltratorGameManager(InfiltratorGameFactory gameFactory)
        {
            GameFactory = gameFactory;
        }

        public InfiltratorGame CurrentGame { get; private set; }

        private InfiltratorGameFactory GameFactory { get; }

        public InfiltratorGame CreateGame(ITextChannel channel)
        {
            CurrentGame = GameFactory.CreateGame(channel);

            return CurrentGame;
        }
    }
}
