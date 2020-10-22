using Discord;
using Discord.WebSocket;

namespace DiscordBot.Infiltrator
{
    public class InfiltratorGameManager
    {
        public InfiltratorGameManager(DiscordSocketClient client)
        {
            Client = client;
        }

        public InfiltratorGame CurrentGame { get; private set; }

        public DiscordSocketClient Client { get; }

        public InfiltratorGame CreateGame(ITextChannel channel)
        {
            CurrentGame = new InfiltratorGame(Client, channel);

            return CurrentGame;
        }
    }
}
