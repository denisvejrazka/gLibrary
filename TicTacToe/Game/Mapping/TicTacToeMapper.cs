using System;
using gLibrary.Mapping;
using gLibrary.Models;

namespace TicTacToe.Game.Mapping;

public class TicTacToeMapper : IMap
{
    public Cell GetMap(int value, int row, int col) 
    {
        return value switch
        {
            0 => new Cell(row, col, "#FFFFFF"),
            1 => new Cell(row, col, "#FFFFFF", "", "avares://TicTacToe/Assets/circle.png"),
            2 => new Cell(row, col, "#FFFFFF", "", "avares://TicTacToe/Assets/cross.png"),
            _ => throw new NotImplementedException()
        };
    }
}
