using Discord;
using Discord.WebSocket;
using DiscordBot.InfiltratorGame.Models;

namespace DiscordBot.InfiltratorGame
{
    public partial class Player
    {
        public class Factory
        {
            private readonly DiscordSocketClient client;

            public Factory(DiscordSocketClient client)
            {
                this.client = client;
            }

            public Player Create(IUser user)
            {
                var data = new PlayerData(user.Id);

                return Create(user, data);
            }

            public Player Create(PlayerData data)
            {
                var user = client.GetUser(data.Id);

                return Create(user, data);
            }

            public Player Create(IUser user, PlayerData data)
            {
                return new Player(user, data);
            }
        }
    }
}
