using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Helping;
using gLibrary.Mapping;
using gLibrary.Models;
using System;
using System.Collections.Generic;
using Avalonia.Platform;
using System.Linq;

namespace gLibrary.Rendering
{
    public class TriangleRenderer : Control
    {
        private readonly IRenderer _renderer;
        private readonly GridEngine _engine;
        private readonly IMap _mapper;
        private readonly TriangleHelper _helper;
        private readonly int _cellSize;

        public TriangleRenderer(IRenderer renderer, GridEngine engine, IMap mapper, TriangleHelper helper, int cellSize)
        {
            _renderer = renderer;
            _engine = engine;
            _mapper = mapper;
            _helper = helper;
            _cellSize = cellSize;
        }

        public void RenderGrid()
        {
            _renderer.Clear();

            for (int row = 0; row < _engine.Rows; row++)
            {
                for (int col = 0; col < _engine.Columns; col++)
                {
                    var value = _engine.GetCellValue(row, col);
                    var cell = _mapper.GetMap(value, row, col);
                    var position = _helper.GetPosition(row, col, _cellSize);
                    _renderer.RenderCell(row, col, cell, _cellSize, position);
                }
            }
        }
    }
}