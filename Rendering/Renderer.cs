using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Mapping;
using System;
namespace gLibrary.Rendering
{
    //Abstraction
    public class Renderer
    {
        protected IRenderer renderer;
        // public event EventHandler<CellClickEventArgs> OnCellClick;
        // public event EventHandler<CellHoverEventArgs>? CellHovered;

        public Renderer(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void RenderGrid(GridEngine engine, IMap mapper, int cellSize)
        {
            renderer.RenderGrid(engine, mapper, cellSize);
        }
    }
}
