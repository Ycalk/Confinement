using System.IO;
using Newtonsoft.Json;

namespace Confinement.GameModel.PlayerProgress
{
    public static class SaveLoadManager
    {
        private static readonly string FilePath = 
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), 
                "playerProgress.json");

        public static void SaveProgress(PlayerProgress progress)
        {
            var json = JsonConvert.SerializeObject(progress, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static PlayerProgress LoadProgress()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<PlayerProgress>(json);
            }
            else
            {
                return new PlayerProgress();
            }
        }
    }
}

