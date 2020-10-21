using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Infiltrator
{
    [Group("RPG")]
    [Summary("Commands for the Infiltrator text-based RPG.")]
    public class InfiltratorGameModule : ModuleBase<SocketCommandContext>
    {
        public static InfiltratorGame game;

        [Command("StartGame")]
        [Summary("Starts a new game of Infiltrator.")]
        public async Task StartGame([Remainder] string name = "Infiltrator")
        {
            await Context.Channel.SendMessageAsync("Starting a new game of Infiltrator.");

            game = new InfiltratorGame();
            game.name = name;
        }

        [Command("Current")]
        [Summary("Gets information about the current game.")]
        public async Task GetCurrent()
        {
            if (game != null)
            {
                await Context.Channel.SendMessageAsync(
                    $"Infiltrator:\n" +
                    $"Game name: {game.name}\n" +
                    $"Difficulty level: {game.difficultyLevel}\n" +
                    $"Player count: {game.players.Count}");
            }
            else
            {
                await Context.Channel.SendMessageAsync("No games active.");
            }
        }
    }

    public class InfiltratorGame
    {
        public string name;

        public List<Player> players = new List<Player>();
        public int difficultyLevel = 0;
    }

    public class Player
    {

    }
}
