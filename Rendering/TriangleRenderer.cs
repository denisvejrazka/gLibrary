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
    public class TriangleRenderer : Control, IRenderer
    {
        private Canvas _canvas;
        private int _cellSize;
        private TriangleHelper _triangleHelper;
        private IMap _mapper;
        private GridEngine _engine;
        private double _zoom = 1.0;
        public event EventHandler<CellClickEventArgs>? CellClicked;
        public event EventHandler<CellHoverEventArgs>? CellHovered;

        public TriangleRenderer(Canvas canvas, int cellSize, double zoom, TriangleHelper triangleHelper, GridEngine engine, IMap mapper, EventHandler<CellClickEventArgs>? OnClick = null, EventHandler<CellHoverEventArgs> OnHover = null)
        {
            _canvas = canvas;
            _cellSize = cellSize;
            _triangleHelper = triangleHelper;
            _engine = engine;
            _mapper = mapper;
            _zoom = zoom;

            //events
            _canvas.PointerPressed += OnPointerPressed;
            _canvas.PointerEntered += OnPointerMoved;
            CellClicked = OnClick;
            CellHovered = OnHover;
        }

        private Control GetCellRepresentation(int row, int col, int cellSize)
        {
            int value = _engine.GetCellValue(row, col);
            Cell cell = _mapper.GetMap(value, row, col);
            var points = new List<Avalonia.Point>();
            double height = Math.Sqrt(3) / 2 * cellSize;
            bool isUpward = (row + col) % 2 == 0;

            if (isUpward)
            {
                points.Add(new Avalonia.Point(0, height));
                points.Add(new Avalonia.Point(cellSize / 2, 0));
                points.Add(new Avalonia.Point(cellSize, height));
            }
            else
            {
                points.Add(new Avalonia.Point(0, 0));
                points.Add(new Avalonia.Point(cellSize, 0));
                points.Add(new Avalonia.Point(cellSize / 2, height));
            }

            var triangle = new Polygon
            {
                Points = points,
                Fill = new SolidColorBrush(Color.Parse(cell.Fill)),
                Stroke = Brushes.Black,
                StrokeThickness = 0.3
            };

            var panel = new Panel { Width = cellSize, Height = cellSize };
            panel.Children.Add(triangle);

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
                FontSize = cellSize * 0.3,
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
                    int value = _engine.GetCellValue(i, j);
                    var cell = mapper.GetMap(value, i, j);
                    var shape = GetCellRepresentation(i, j, cellSize);
                    var (xOffset, yOffset) = _triangleHelper.GetPosition(i, j, cellSize);

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
            var (xOffset, yOffset) = _triangleHelper.GetPosition(row, col, _cellSize);

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
            var cellCoords = _triangleHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
            Cell cell = _mapper.GetMap(_engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col), cellCoords.Value.row, cellCoords.Value.col);

            if (cell != null)
            {
                var mouseButton = e.GetCurrentPoint(_canvas).Properties.IsLeftButtonPressed ? MouseButton.Left : MouseButton.Right;
                CellClicked?.Invoke(this, new CellClickEventArgs(cell, mouseButton));
            }
        }


        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            var point = e.GetPosition(_canvas);
            var cellCoords = _triangleHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
            Cell cell = _mapper.GetMap(_engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col), cellCoords.Value.row, cellCoords.Value.col);

            if (cell != null)
            {
                CellHovered?.Invoke(this, new CellHoverEventArgs(cell, e));
            }
        }
    }
}