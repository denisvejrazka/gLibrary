using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Mapping;
using System;

namespace gLibrary.Rendering
{
    public interface IRenderer
    {
        event EventHandler<CellClickEventArgs> CellClicked;
        event EventHandler<CellHoverEventArgs>? CellHovered;
        void UpdateCell(int row, int col);
        void RenderGrid(GridEngine engine, IMap mapper, int cellSize);
    }
}