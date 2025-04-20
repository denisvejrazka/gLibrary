using Avalonia.Controls;
using gLibrary.Engine;
using gLibrary.Helping;
using gLibrary.Rendering;
using Tri.Game;
using Tri.Game.Mapping;
using System;
using Avalonia.Input;
using gLibrary.Events;
using System.Collections.Generic;
using System.Linq;

namespace Tri.Views
{
    public partial class MainWindow : Window
    {
        private GridEngine _engine;
        private Renderer _renderer;
        private TriangleHelper _triangleHelper;
        private TriangleRenderer _triangleRenderer;
        private TriMapper _mapper;
        private TriGameController _gameController;
        private int _size;
        private bool _cleared;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _size = 70;
            _engine = new GridEngine(4, 2);
            _engine.GenerateGrid();
            _cleared = false;

            for (int i = 0; i < _engine.Rows; i++)
            {
                for (int j = 0; j < _engine.Columns; j++)
                {
                    _engine.SetCellValue(i, j, 2);
                    _cleared = true;
                }
            }

            // var predefinedPairs = new (int row, int col, int dRow, int dCol, int value)[]
            // {
            //     (0, 0, 0, 1, 0), // 1-1 vodorovně
            //     (0, 2, 1, 0, 0), // 0-0 svisle
            //     (1, 1, 0, 1, 1), // 1-1 vodorovně
            //     (2, 0, 1, 0, 0), // 0-0 svisle
            // };

            // foreach (var (row, col, dRow, dCol, value) in predefinedPairs)
            // {
            //     int r1 = row;
            //     int c1 = col;
            //     int r2 = row + dRow;
            //     int c2 = col + dCol;

            //     if (IsInside(r1, c1) && IsInside(r2, c2))
            //     {
            //         _engine.SetCellValue(r1, c1, value);
            //         _engine.SetCellValue(r2, c2, value);
            //     }
            // }

            _mapper = new TriMapper();
            _triangleHelper = new TriangleHelper(_engine);
            _triangleRenderer = new TriangleRenderer(TriBackground, _size, 0.5, _triangleHelper, _engine, _mapper, OnClick);
            _gameController = new TriGameController(_engine, _triangleHelper);
            _renderer = new Renderer(_triangleRenderer);
            _renderer.RenderGrid(_engine, _mapper, _size);
        }

        // public void OnClick(object? sender, CellClickEventArgs args)
        // {
        //     int row = args.Cell.Row;
        //     int col = args.Cell.Column;

        //     int value = args.MouseButton switch
        //     {
        //         MouseButton.Left => 1,
        //         MouseButton.Right => 0,
        //         _ => 2
        //     };

        //     int initialValue = _engine.GetCellValue(row, col);
        //     List<int> neighborValues = new List<int>();
        //     List<(int, int)> neighbors = _triangleHelper.GetNeighbors(row, col);
            
        //     foreach (var (nRow, nCol) in neighbors)
        //     {
        //         neighborValues.Add(_engine.GetCellValue(nRow, nCol));
        //     }

        //     if (neighborValues.Contains(initialValue) && neighborValues.RemoveAll(n => n == 0) && neighbors.Count == 3)
        //     {
        //         _engine.SetCellValue(row, col, value);
        //     }
        // }
        
        public void OnClick(object? sender, CellClickEventArgs args)
        {
            int row = args.Cell.Row;
            int col = args.Cell.Column;

            int newValue = args.MouseButton switch
            {
                MouseButton.Left => 1,
                MouseButton.Right => 0,
                _ => 2
            };

            int currentValue = _engine.GetCellValue(row, col);

            var neighbors = _triangleHelper.GetNeighbors(row, col);
            int sameValueCount = 0;

            foreach (var (nRow, nCol) in neighbors)
            {
                int neighborValue = _engine.GetCellValue(nRow, nCol);
                if (neighborValue == currentValue)
                {
                    sameValueCount++;
                }
            }

            // Pokud má právě jednoho souseda se stejnou hodnotou, nastav novou hodnotu
            if (sameValueCount == 1)
            {
                _engine.SetCellValue(row, col, newValue);
                _triangleRenderer.UpdateCell(row, col);
            }
        }
    }
}