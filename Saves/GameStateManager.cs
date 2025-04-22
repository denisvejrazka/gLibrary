using System;
using System.IO;
using System.Text.Json;
using gLibrary.Saves;
namespace gLibrary.Saves
{
    public class GameStateManager
    {
        private const string DefaultSavePath = "Saves/saved_game.json";

        public void SaveGame(GridState state, string? filePath = null)
        {
            string path = filePath ?? DefaultSavePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }

        public GridState? LoadGame(string? filePath = null)
        {
            string path = filePath ?? DefaultSavePath;

            if (!File.Exists(path))
                return null;

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GridState>(json);
        }
    }
}