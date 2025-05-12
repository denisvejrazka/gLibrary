using Avalonia.Controls;
using gLibrary.Engine;
using gLibrary.Events;
using gLibrary.Helping;
using gLibrary.Rendering;
using MemTest.Game.Mapping;
using MemTest.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gLibrary.Rendering.AvaloniaRenderers;
//ještě jednu hru
//ve vytvoreni abstraktniho modelu popsat jen obecne oddeleni, proc gridengine je obecny atd...
namespace MemTest.Views;

public partial class MainWindow : Window
{
    private GridEngine _engine;
    private AvaloniaSquareRenderer _avaloniaSquareRenderer;
    private SquareHelper _squareHelper;
    private SquareRenderer _squareRenderer;
    private MemMapper _mapper;
    private int _size;
    public Random random = new Random();
    private int _score;
    private List<(int Row, int Col)> _sequence = new();
    private List<(int Row, int Col)> _playerInput = new();
    private int _currentStep = 0;
    private bool _isPlayerTurn = false;

    public MainWindow()
    {
        InitializeComponent();
        _size = 70;
        _engine = new GridEngine(3, 3);
        _engine.GenerateGrid();
        int rows = _engine.Rows;
        int cols = _engine.Columns;

        int canvasWidth = cols * _size;
        int canvasHeight = rows * _size;
        MemBackground.Width = canvasWidth;
        MemBackground.Height = canvasHeight;
        this.Width = canvasWidth + 80; 
        this.Height = canvasHeight + 80;
        
        InitializeGrid();
        StartGame();
    }

    private void InitializeGrid()
    {
        for (int i = 0; i < _engine.Rows; i++)
            for (int j = 0; j < _engine.Columns; j++)
                _engine.SetCellValue(i, j, 0);

        _mapper = new MemMapper();
        _avaloniaSquareRenderer = new AvaloniaSquareRenderer(MemBackground);
        _squareHelper = new SquareHelper(_engine);
        _squareRenderer = new SquareRenderer(_avaloniaSquareRenderer, _engine, _mapper , _squareHelper, _size);
        _squareRenderer.RenderGrid();
    }

    public async void OnClick(object? sender, CellClickEventArgs args)
    {
        if (!_isPlayerTurn) return;
        int row = args.Cell.Row;
        int col = args.Cell.Column;
        _playerInput.Add((row, col));
        _engine.SetCellValue(row, col, 1);
        _avaloniaSquareRenderer.UpdateCell(row, col);
        await Task.Delay(200);
        _engine.SetCellValue(row, col, 0);
        _avaloniaSquareRenderer.UpdateCell(row, col);

        if (_sequence[_currentStep] != (row, col))
        {
            EndGame("Špatné kliknutí! Skóre: " + _score);
            return;
        }

        _currentStep++;

        // done?
        if (_currentStep >= _sequence.Count)
        {
            _score++;
            AddRandomCellToSequence();
            await Task.Delay(500);
            await ShowSequenceAsync();
        }
    }

    private async Task ShowSequenceAsync()
    {
        _isPlayerTurn = false;
        foreach (var (row, col) in _sequence)
        {
            _engine.SetCellValue(row, col, 1);
            _avaloniaSquareRenderer.UpdateCell(row, col);
            await Task.Delay(600);

            _engine.SetCellValue(row, col, 0);
            _avaloniaSquareRenderer.UpdateCell(row, col);
            await Task.Delay(200);
        }

        _playerInput.Clear();
        _currentStep = 0;
        _isPlayerTurn = true;
    }

    private void AddRandomCellToSequence()
    {
        int row = random.Next(0, _engine.Rows);
        int col = random.Next(0, _engine.Columns);
        _sequence.Add((row, col));
    }

    private void EndGame(string message)
    {
        _isPlayerTurn = false;
        EndGameMessage.Text = message;
        EndGameMessage.IsVisible = true;
    }

    private async void StartGame()
    {
        EndGameMessage.IsVisible = false;
        _score = 0;
        _sequence.Clear();
        AddRandomCellToSequence();
        await ShowSequenceAsync();
    }
}