using System.Text.Json;
using System.IO;
using gLibrary.Engine;

namespace gLibrary.Saves
{
    public class GridState
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<List<int>> GridValues { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}