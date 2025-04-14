using gLibrary.Models;
namespace gLibrary.Mapping
{
    public class Mapper : IMap
    {   
        public Cell GetMap(int value, int row, int col) 
        {
            return value switch
            {
                0 => new Cell(row, col, "#FFFFFF"), 
                1 => new Cell(row, col, "#f3fc9d"),
                2 => new Cell(row, col, "#d5f0d8"),
                3 => new Cell(row, col, "#d5e6f0"),
                4 => new Cell(row, col, "#f7a6f5"),
                _ => new Cell(row, col, "#CCCCCC")
            };
        }
    }
}