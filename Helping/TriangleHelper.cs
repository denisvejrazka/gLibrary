using System;
using gLibrary.Engine;
using System.Collections.Generic;

namespace gLibrary.Helping
{
    public class TriangleHelper : Helper
    {
        public TriangleHelper(GridEngine engine) : base(engine) { }

        public override (double x, double y) GetPosition(int row, int col, int cellSize)
        {
            double height = cellSize * Math.Sqrt(3) / 2;
            double xOffset = col * (cellSize * 0.5);
            double yOffset = row * height;

            return (xOffset, yOffset);
        }

        public override (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize)
        {
            double triHeight = cellSize * Math.Sqrt(3) / 2;
            double triWidth = cellSize;

            for (int row = 0; row < Engine.Rows; row++)
            {
                for (int col = 0; col < Engine.Columns; col++)
                {
                    var (cellX, cellY) = GetPosition(row, col, cellSize);

                    var points = new List<Avalonia.Point>();
                    bool isUpward = (row + col) % 2 == 0;

                    if (isUpward)
                    {
                        points.Add(new Avalonia.Point(cellX, cellY + triHeight));
                        points.Add(new Avalonia.Point(cellX + cellSize / 2, cellY)); 
                        points.Add(new Avalonia.Point(cellX + cellSize, cellY + triHeight)); 
                    }
                    else
                    {
                        points.Add(new Avalonia.Point(cellX, cellY)); 
                        points.Add(new Avalonia.Point(cellX + cellSize, cellY)); 
                        points.Add(new Avalonia.Point(cellX + cellSize / 2, cellY + triHeight)); 
                    }

                    if (IsPointInTriangle(new Avalonia.Point(x, y), points))
                    {
                        return (row, col);
                    }
                }
            }
            return null;
        }

        private bool IsPointInTriangle(Avalonia.Point p, List<Avalonia.Point> triangle)
        {
            var A = triangle[0];
            var B = triangle[1];
            var C = triangle[2];

            double cross1 = (B.X - A.X) * (p.Y - A.Y) - (B.Y - A.Y) * (p.X - A.X);
            double cross2 = (C.X - B.X) * (p.Y - B.Y) - (C.Y - B.Y) * (p.X - B.X);
            double cross3 = (A.X - C.X) * (p.Y - C.Y) - (A.Y - C.Y) * (p.X - C.X);

            bool hasNegative = (cross1 < 0) || (cross2 < 0) || (cross3 < 0);
            bool hasPositive = (cross1 > 0) || (cross2 > 0) || (cross3 > 0);

            return !(hasNegative && hasPositive); 
        }

        public override List<(int row, int col)> GetNeighbors(int row, int col)
        {
            var neighbors = new List<(int row, int col)>();
            var directions = new (int, int)[] { (0, -1), (0, 1), ((col + row) % 2 == 0 ? (1, 0) : (-1, 0))};

            foreach (var (directionRow, directionCol) in directions)
            {
                int neighborRow = row + directionRow, neighborCol = col + directionCol;
                if (neighborRow >= 0 && neighborRow < Engine.Rows && neighborCol >= 0 && neighborCol < Engine.Columns)
                {
                    neighbors.Add((neighborRow, neighborCol));
                }
            }

            return neighbors;
        }
    }
}