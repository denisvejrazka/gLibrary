using System;
using System.IO;
using System.Text.Json;

namespace gLibrary.Saves;

public class GameStateManager
{
    private const string SaveFilePath = "Saves/GameStateManager.cs/saved_game.json";

    public void SaveGame(GameState state)
    {
        string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SaveFilePath, json);
    }

    public GameState? LoadGame()
    {
        if (!File.Exists(SaveFilePath))
            return null;

        string json = File.ReadAllText(SaveFilePath);
        return JsonSerializer.Deserialize<GameState>(json);
    }
}
