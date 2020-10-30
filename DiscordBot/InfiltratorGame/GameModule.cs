using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Extensions;

namespace DiscordBot.InfiltratorGame
{
    [Group("RPG")]
    [Summary("Commands for the Infiltrator text-based RPG.")]
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        private readonly GameManager gameManager;

        public GameModule(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        [Command("StartGame"), Alias("Start")]
        [Summary("Starts a new game of Infiltrator in the current channel.")]
        public async Task StartGame()
        {
            await Context.Channel.SendMessageAsync("Starting a new game of Infiltrator.");
            gameManager.CreateGame((ITextChannel)Context.Channel).CreateAndShowNewEnemy().Forget();
        }

        [Command("GameInfo"), Alias("Game")]
        [Summary("Shows information about the current game in the channel.")]
        public async Task GetGameInfo()
        {
            if (gameManager.Games.TryGetValue(Context.Channel.Id, out Game game))
            {
                await Context.Channel.SendMessageAsync(embed: game.ToEmbed());
            }
            else
            {
                await Context.Channel.SendMessageAsync("No games active in this channel.");
            }
        }

        [Command("PlayerInfo"), Alias("Player")]
        [Summary("Shows information the specified player.")]
        public async Task GetPlayerInfo(IUser user = null)
        {
            user = user ?? Context.User;

            if (gameManager.Games.TryGetValue(Context.Channel.Id, out Game game) && game.PlayersById.TryGetValue(user.Id, out Player player))
            {
                await Context.Channel.SendMessageAsync(embed: player.ToEmbed());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Player not found in current game.");
            }
        }

        [Command("Serialize")]
        [Summary("Serializes the game and returns current game data in JSON format.")]
        public async Task Serialize()
        {
            string json = gameManager.ToJson();

            await Context.Channel.SendMessageAsync($"```json\n{json}\n```");
        }
    }
}
