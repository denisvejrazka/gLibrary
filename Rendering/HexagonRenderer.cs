using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Helping;
using gLibrary.Mapping;
using gLibrary.Models;
using System;
using System.Linq;

//abstract renderer
namespace gLibrary.Rendering
{
    public class HexagonRenderer
    {
        private readonly IRenderer _renderer;
        private readonly GridEngine _engine;
        private readonly IMap _mapper;
        private readonly HexagonHelper _helper;
        private readonly int _cellSize;

        public HexagonRenderer(IRenderer renderer, GridEngine engine, IMap mapper, HexagonHelper helper, int cellSize)
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
