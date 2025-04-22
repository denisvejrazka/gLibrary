using System;

namespace gLibrary.Engine
{
    public class GridEngine
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int[,] Matrix { get; private set; }

        public GridEngine(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.Matrix = new int[Rows, Columns];
        }


        public void GenerateGrid()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    Matrix[i, j] = 0;
        }
        

        public void SetCellValue(int row, int col, int value)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
                Matrix[row, col] = value;
        }

        
        public int GetCellValue(int row, int col) => (row >= 0 && row < Rows && col >= 0 && col < Columns) 
            ? Matrix[row, col] : throw new IndexOutOfRangeException("Invalid cell coordinates.");

        public int[,] ExportGrid()
        {
            var copy = new int[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    copy[i, j] = GetCellValue(i, j);
                }
            }
            return copy;
        }

        public void SetGrid(int[,] newGrid)
        {
            if (newGrid.GetLength(0) != Rows || newGrid.GetLength(1) != Columns)
                throw new ArgumentException("Grid size does not match.");

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    SetCellValue(i, j, newGrid[i, j]);
                }
            }
        }
    }
}
