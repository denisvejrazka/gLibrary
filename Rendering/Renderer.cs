using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Mapping;
using System;
namespace gLibrary.Rendering
{
    //Abstraction
    public class Renderer
    {
        protected IRenderer _renderer;
        public event EventHandler<CellClickEventArgs> OnCellClick;
        public event EventHandler<CellHoverEventArgs>? CellHovered;

        public Renderer(IRenderer renderer)
        {
            this._renderer = renderer;
            //this._renderer.CellClicked += (sender, args) => OnCellClick?.Invoke(sender, args);
        }

        public void RenderGrid(GridEngine engine, IMap mapper, int cellSize)
        {
            _renderer.RenderGrid(engine, mapper, cellSize);
        }
    }
}
