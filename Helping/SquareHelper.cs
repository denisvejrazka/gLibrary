using gLibrary.Engine;
using System.Collections.Generic;

namespace gLibrary.Helping
{
    public class SquareHelper : Helper
    {
        public SquareHelper(GridEngine engine) : base(engine) { }

        public override (double x, double y) GetPosition(int row, int col, int cellSize)
        {
            return (col * cellSize, row * cellSize);
        }

        public override (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize)
        {
            int col = (int)(x / cellSize);
            int row = (int)(y / cellSize);
            
            if (row < 0 || row >= Engine.Rows || col < 0 || col >= Engine.Columns)
            {
                return null;
            }

            return (row,col);
        }

        public override List<(int row, int col)> GetNeighbors(int row, int col)
        {
            var neighbors = new List<(int row, int col)>();

            List<(int, int)> directions = new(){(-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)};

            foreach (var (directionRow, directionCol) in directions)
            {
                int neighborRow = row + directionRow;
                int neighborCol = col + directionCol;

                if (neighborRow >= 0 && neighborRow < Engine.Rows && neighborCol >= 0 && neighborCol < Engine.Columns)
                {
                    neighbors.Add((neighborRow, neighborCol));
                }
            }
            return neighbors;
        }
    }
}