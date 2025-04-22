using System;
using gLibrary.Mapping;
using gLibrary.Models;

namespace MemTest.Game.Mapping;

public class MemMapper : IMap
{
    public Cell GetMap(int value, int row, int col) 
    {
        return value switch
        {
            0 => new Cell(row, col, "#FFFFFF"),
            1 => new Cell(row, col, "#3c00ff"),
            _ => throw new NotImplementedException()
        };
    }
}
