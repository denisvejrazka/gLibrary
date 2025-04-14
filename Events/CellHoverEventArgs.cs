using System;
using Avalonia.Input;
using gLibrary.Models;

namespace gLibrary.Events
{
    public class CellHoverEventArgs: EventArgs
    {
        public Cell Cell { get; }
        public PointerEventArgs PointerEventArgs { get; }

        public CellHoverEventArgs(Cell cell , PointerEventArgs pointerEventArgs)
        {
            Cell = cell;
            PointerEventArgs = pointerEventArgs;
        }
    }
}
