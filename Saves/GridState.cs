using System.Text.Json;
using System.IO;
using gLibrary.Engine;

namespace gLibrary.Saves
{
    public class GameState
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int[,] GridValues { get; set; }
    }
}