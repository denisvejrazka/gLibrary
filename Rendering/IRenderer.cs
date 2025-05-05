using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Mapping;
using gLibrary.Models;
using System;

namespace gLibrary.Rendering
{
    public interface IRenderer
    {
        void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position);
        void Clear();
    }
}