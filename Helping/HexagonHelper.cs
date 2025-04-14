using System;
using System.Collections.Generic;
using gLibrary.Engine;

namespace gLibrary.Helping
{
    public class HexagonHelper : Helper
    {
        public HexagonHelper(GridEngine engine) : base(engine) { }

        public override (double x, double y) GetPosition(int row, int col, int cellSize)
        {
            double width = cellSize * 1.5;
            double height = Math.Sqrt(3) * cellSize;
            double xOffset = col * width;
            double yOffset = row * height;

            if (col % 2 == 1)
                yOffset += height / 2;

            return (xOffset, yOffset);
        }

        public override (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize)
        {
            double width = cellSize * 1.5;
            double height = Math.Sqrt(3) * cellSize;
            int col = (int)(x / width);
            int row = (int)((y - (col % 2 == 1 ? height / 2 : 0)) / height);
            if (row < 0 || row >= Engine.Rows || col < 0 || col >= Engine.Columns)
            {
                return null;
            }

            return (row, col);
        }

        public override List<(int, int)> GetNeighbors(int row, int col)
        {
            var neighbors = new List<(int, int)>();

            var directions = (col % 2 == 0) 
            ? new (int, int)[] { 
                (-1, 0),  // horní soused
                (1, 0),   // dolní soused
                (-1, -1), // horní levý soused
                (0, -1),  // levý soused
                (0, 1),   // pravý soused
                (-1, 1)   // horní pravý soused
            }
            : new (int, int)[] { 
                (1, 0),   // dolní soused
                (-1, 0),  // horní soused
                (1, -1),  // dolní levý soused
                (0, -1),  // levý soused
                (0, 1),   // pravý soused
                (1, 1)    // dolní pravý soused
            };

            foreach (var (directionRow, directionCol) in directions)
            {
                int neighborRow = row + directionRow;
                int neighborCol = col + directionCol;

                if (neighborRow >= 0 && neighborRow < Engine.Rows && neighborCol >= 0 && neighborCol < Engine.Columns)
                {
                    var neighbor = (neighborRow, neighborCol);
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

    }
}