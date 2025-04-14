using System.Collections.Generic;
using gLibrary.Engine;

namespace gLibrary.Helping
{
    public abstract class Helper
    {
        protected GridEngine Engine;
        protected Helper(GridEngine engine){Engine = engine;}
        public abstract (double x, double y) GetPosition(int row, int col, int cellSize);
        public abstract (int row, int col)? GetCellCoordinatesFromPixel(double x, double y, int cellSize);
        public abstract List<(int row, int col)> GetNeighbors(int row, int col);
    }
}
