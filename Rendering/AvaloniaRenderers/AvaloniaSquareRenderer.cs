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

namespace gLibrary.Rendering.AvaloniaRenderers;

// add UpdateCell(int row, int col)
public class AvaloniaSquareRenderer : Control, IRenderer
{
    private readonly Canvas _canvas;
    private SquareHelper _squareHelper;
    private GridEngine _engine;
    private int _cellSize;
    private IMap _mapper;

    //events
    public event EventHandler<CellClickEventArgs>? CellClicked;
    public event EventHandler<CellHoverEventArgs>? CellHovered;
    private readonly Dictionary<(int row, int col), Panel> _cellVisuals = new();

    public AvaloniaSquareRenderer(Canvas canvas, GridEngine engine, IMap mapper, SquareHelper squareHelper, int cellSize, EventHandler<CellClickEventArgs>? OnClick = null, EventHandler<CellHoverEventArgs>? OnHover = null)
    {
        _canvas = canvas;
        //events
        _canvas.PointerPressed += OnPointerPressed;
        //_canvas.PointerEntered += OnPointerMoved;
        CellClicked = OnClick;
        CellHovered = OnHover;
        _engine = engine;
        _mapper = mapper;
        _squareHelper = squareHelper;
        _cellSize = cellSize;
    }

    public void Clear() => _canvas.Children.Clear();

    public void RenderCell(int row, int col, Cell cell, int cellSize, (double x, double y) position)
    {
        var rect = new Rectangle
        {
            Width = cellSize,
            Height = cellSize,
            Fill = new SolidColorBrush(Color.Parse(cell.Fill)),
            Stroke = Brushes.Black,
            StrokeThickness = 0.5,
        };

        var panel = new Panel { Width = cellSize, Height = cellSize };
        panel.Children.Add(rect);

        if (!string.IsNullOrEmpty(cell.Raster))
        {
            var uri = new Uri(cell.Raster);
            var bitmapImage = new Image
            {
                Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(uri)),
                Width = cellSize,
                Height = cellSize,
            };
            panel.Children.Add(bitmapImage);
        }

        var textBlock = new TextBlock
        {
            Text = cell.Text,
            Foreground = Brushes.Black,
            FontSize = cellSize * 0.4,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        };

        panel.Children.Add(textBlock);

        Canvas.SetLeft(panel, position.x);
        Canvas.SetTop(panel, position.y);
        _canvas.Children.Add(panel);

        // Uložení panelu pro možnost pozdější aktualizace
        _cellVisuals[(row, col)] = panel;
    }

    public void UpdateCell(int row, int col)
    {
        var value = _engine.GetCellValue(row, col);

        var cell = _mapper.GetMap(value, row, col);

        var position = _squareHelper.GetPosition(row, col, _cellSize);

        if (_cellVisuals.TryGetValue((row, col), out var oldPanel))
        {
            _canvas.Children.Remove(oldPanel);
            _cellVisuals.Remove((row, col));
        }

        RenderCell(row, col, cell, _cellSize, position);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(_canvas);
        var cellCoords = _squareHelper.GetCellCoordinatesFromPixel(point.X, point.Y, _cellSize);
        Cell cell = _mapper.GetMap(
            _engine.GetCellValue(cellCoords.Value.row, cellCoords.Value.col),
            cellCoords.Value.row,
            cellCoords.Value.col
        );

        if (cell != null)
        {
            var mouseButton = e.GetCurrentPoint(_canvas).Properties.IsLeftButtonPressed
                ? MouseButton.Left
                : MouseButton.Right;
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