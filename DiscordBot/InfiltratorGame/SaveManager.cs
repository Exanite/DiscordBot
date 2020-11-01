using System.IO;
using System.Text;
using DiscordBot.Logging;

namespace DiscordBot.InfiltratorGame
{
    // todo currently has some code repetition, try to find a way to remove it
    public class SaveManager
    {
        public const string SaveFolderPath = @"Data\Saves";

        public const string GameJsonFileName = @"Games.json";
        public const string PlayerJsonFileName = @"Players.json";

        private readonly ILog<SaveManager> log;
        private readonly GameManager gameManager;
        private readonly PlayerManager playerManager;

        public SaveManager(ILog<SaveManager> log, GameManager gameManager, PlayerManager playerManager)
        {
            this.log = log;
            this.gameManager = gameManager;
            this.playerManager = playerManager;
        }

        public string GamesFilePath => Path.Combine(SaveFolderPath, GameJsonFileName);
        public string PlayersFilePath => Path.Combine(SaveFolderPath, PlayerJsonFileName);

        public void Save()
        {
            var saveFolder = new DirectoryInfo(SaveFolderPath);
            saveFolder.Create();

            var gameJsonFile = new FileInfo(GamesFilePath);
            using (var stream = gameJsonFile.CreateText())
            {
                string json = gameManager.SaveToJson();

                log.Debug("Saved game json:\n{Json}", json);

                stream.Write(json);
            }

            var playerJsonFile = new FileInfo(PlayersFilePath);
            using (var stream = playerJsonFile.CreateText())
            {
                string json = playerManager.SaveToJson();

                log.Debug("Saved player json:\n{Json}", json);

                stream.Write(json);
            }
        }

        public void Load()
        {
            var gameJsonFile = new FileInfo(GamesFilePath);
            if (gameJsonFile.Exists)
            {
                using (var stream = gameJsonFile.OpenText())
                {
                    string json = stream.ReadToEnd();

                    log.Debug("Loaded game json:\n{Json}", json);

                    gameManager.LoadFromJson(json);
                }
            }

            var playerJsonFile = new FileInfo(PlayersFilePath);
            if (playerJsonFile.Exists)
            {
                using (var stream = playerJsonFile.OpenText())
                {
                    string json = stream.ReadToEnd();

                    log.Debug("Loaded player json:\n{Json}", json);

                    playerManager.LoadFromJson(json);
                }
            }
        }
    }
}
