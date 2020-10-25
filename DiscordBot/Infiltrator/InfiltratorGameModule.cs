using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Extensions;

namespace DiscordBot.Infiltrator
{
    [Group("RPG")]
    [Summary("Commands for the Infiltrator text-based RPG.")]
    public class InfiltratorGameModule : ModuleBase<SocketCommandContext>
    {
        private readonly InfiltratorGameManager gameManager;

        public InfiltratorGameModule(InfiltratorGameManager gameManager)
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

        [Command("Current")]
        [Summary("Shows information about the current game.")]
        public async Task GetCurrent()
        {
            var game = gameManager.CurrentGame;

            if (game != null)
            {
                await Context.Channel.SendMessageAsync(embed: game.ToEmbed());
            }
            else
            {
                await Context.Channel.SendMessageAsync("No games active.");
            }
        }
    }
}
