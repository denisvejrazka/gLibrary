using Avalonia.Input;
using gLibrary.Models;
using System;

namespace gLibrary.Events
{
    public class CellClickEventArgs : EventArgs
    {
        public Cell Cell { get; }
        public MouseButton MouseButton { get; }

        public CellClickEventArgs(Cell cell, MouseButton mouseButton)
        {
            Cell = cell;
            MouseButton = mouseButton;
        }
    }
}
