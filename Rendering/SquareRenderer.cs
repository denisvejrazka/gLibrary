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

namespace gLibrary.Rendering
{
    public class SquareRenderer : Control, IRenderer
    {
        private Canvas _canvas;
        private int _cellSize;
        private SquareHelper _squareHelper;
        private IMap _mapper;
        private GridEngine _engine;
        private double _zoom = 1.0;
        private (int row, int col)? _hoveredCell = null;
        
        //events
        public event EventHandler<CellClickEventArgs>? CellClicked;
        public event EventHandler<CellHoverEventArgs>? CellHovered;
        
        public SquareRenderer(Canvas canvas, int cellSize, double zoom, SquareHelper squareHelper, GridEngine engine, IMap mapper, EventHandler<CellClickEventArgs>? OnClick = null, EventHandler<CellHoverEventArgs>? OnHover = null)
        {
            _canvas = canvas;
            _cellSize = cellSize;
            _squareHelper = squareHelper;
            _engine = engine;
            _mapper = mapper;
            _zoom = zoom;
            
            //events
            _canvas.PointerPressed += OnPointerPressed;
            //_canvas.PointerEntered += OnPointerMoved;
            CellClicked = OnClick;
            CellHovered = OnHover;

        }
        
        private Control GetCellRepresentation(int row, int col, int cellSize)
        {
            int value = _engine.GetCellValue(row, col);
            Cell cell = _mapper.GetMap(value, row, col);

            string fill = cell.Fill;

            var rect = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = new SolidColorBrush(Color.Parse(fill)),
                Stroke = Brushes.Black,
                StrokeThickness = 0.5
            };

            var panel = new Panel { Width = cellSize, Height = cellSize };
            panel.Children.Add(rect);

            string? bitmapPath = cell.Raster;
            if (!string.IsNullOrEmpty(bitmapPath))
            {
                var uri = new Uri(bitmapPath);
                var bitmapImage = new Image
                {
                    Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(uri)),
                    Width = cellSize * _zoom,
                    Height = cellSize * _zoom
                };
                panel.Children.Add(bitmapImage);
            }

            var textBlock = new TextBlock
            {
                Text = cell.Text.ToString(),
                Foreground = Brushes.Black,
                FontSize = cellSize * 0.4,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            panel.Children.Add(textBlock);

            return panel;
        }


        public void RenderGrid(GridEngine engine, IMap mapper, int cellSize)
        {
            _canvas.Children.Clear();

            for (int i = 0; i < engine.Rows; i++)
            {
                for (int j = 0; j < engine.Columns; j++)
                {
                    int value = engine.GetCellValue(i,j);
                    Cell cell = mapper.GetMap(value, i, j);
                    var shape = GetCellRepresentation(i, j, cellSize);
                    var (xOffset, yOffset) = _squareHelper.GetPosition(i, j, cellSize);

                    Canvas.SetLeft(shape, xOffset);
                    Canvas.SetTop(shape, yOffset);
                    _canvas.Children.Add(shape);
                }
            }
        }

        public void UpdateCell(int row, int col)
        {
            var shape = GetCellRepresentation(row, col, _cellSize);
            int value = _engine.GetCellValue(row, col);
            var (xOffset, yOffset) = _squareHelper.GetPosition(row, col, _cellSize);

            // find exsiting item on given position
            var oldShape = _canvas.Children.OfType<Control>().FirstOrDefault(c => Canvas.GetLeft(c) == xOffset && Canvas.GetTop(c) == yOffset);

            if (oldShape != null)
                _canvas.Children.Remove(oldShape);

            Canvas.SetLeft(shape, xOffset);
            Canvas.SetTop(shape, yOffset);
            _canvas.Children.Add(shape);
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(_canvas);
            var cellCoords = _squareHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
            Cell cell = _mapper.GetMap(_engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col), cellCoords.Value.row, cellCoords.Value.col);

            if (cell != null)
            {
                var mouseButton = e.GetCurrentPoint(_canvas).Properties.IsLeftButtonPressed ? MouseButton.Left : MouseButton.Right;
                CellClicked?.Invoke(this, new CellClickEventArgs(cell, mouseButton));
            }
        }


        //private void OnPointerMoved(object? sender, PointerEventArgs e)
        //{
        //    var point = e.GetPosition(_canvas);
        //    var cellCoords = _squareHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
        //    Cell cell = _mapper.GetMap(_engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col), cellCoords.Value.row, cellCoords.Value.col);

        //    if (cell != null)
        //    {
        //        CellHovered?.Invoke(this, new CellHoverEventArgs(cell, e));
        //    }
        //}
    }
}
