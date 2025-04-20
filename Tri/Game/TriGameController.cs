using System;
using System.Collections.Generic;
using gLibrary.Engine;
using gLibrary.Helping;

namespace Tri.Game
{
    public class TriGameController
    {
        private GridEngine _engine;
        private TriangleHelper _helper;
        private bool[,] _visited;
        private bool _gameOver;

        public TriGameController(GridEngine engine, TriangleHelper helper)
        {
            _engine = engine;
            _helper = helper;
            _visited = new bool[engine.Rows, engine.Columns];
            _gameOver = false;
        }

        public bool MakeMove(int row, int col, int value)
        {
            if (_gameOver)
                return false;

            _engine.SetCellValue(row, col, value);
            _visited[row, col] = true;

            // Seznam všech buněk, které je potřeba zkontrolovat (tah a jeho sousedé)
            var toCheck = new List<(int r, int c)> { (row, col) };
            toCheck.AddRange(_helper.GetNeighbors(row, col));

            foreach (var (r, c) in toCheck)
            {
                if (_engine.GetCellValue(r, c) != 0 && _engine.GetCellValue(r, c) != 1)
                    continue;

                if (!IsValidCell(r, c))
                {
                    _gameOver = true;
                    return false;
                }
            }

            return true;
        }

        private bool IsValidCell(int row, int col)
        {
            int value = _engine.GetCellValue(row, col);
            var neighbors = _helper.GetNeighbors(row, col);

            int sameCount = 0;
            foreach (var (nRow, nCol) in neighbors)
            {
                if (nRow >= 0 && nRow < _engine.Rows && nCol >= 0 && nCol < _engine.Columns)
                {
                    if (_engine.GetCellValue(nRow, nCol) == value)
                    {
                        sameCount++;
                    }
                }
            }

            return sameCount == 1; // Každý trojúhelník má mít právě 1 souseda se stejnou hodnotou
        }

        public bool IsGameOver() => _gameOver;
    }
}