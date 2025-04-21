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
using System.Reactive.Concurrency;

namespace Tri.Views
{
    public partial class MainWindow : Window
    {
        private GridEngine _engine;
        private Renderer _renderer;
        private TriangleHelper _triangleHelper;
        private TriangleRenderer _triangleRenderer;
        private TriMapper _mapper;
        private int _size;
        public Random random = new Random();
        private int _score = 0;

        private List<(int, int)> blacklist;
        private List<(int, int)> notPrimes;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            _size = 70;
            _engine = new GridEngine(15, 25);
            _engine.GenerateGrid();

            blacklist = new List<(int, int)>();
            
            for (int i = 0; i < _engine.Rows; i++)
            {
                for (int j = 0; j < _engine.Columns; j++)
                {
                    _engine.SetCellValue(i, j, 0);
                }
            }

            _mapper = new TriMapper();
            _triangleHelper = new TriangleHelper(_engine);
            _triangleRenderer = new TriangleRenderer(TriBackground, _size, 0.5, _triangleHelper, _engine, _mapper, OnClick);
            _renderer = new Renderer(_triangleRenderer);
            _renderer.RenderGrid(_engine, _mapper, _size);
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;

            int boundary = (int)Math.Sqrt(number);

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        private void UpdateScoreText()
        {
            ScoreText.Text = $"Skóre: {_score}";
        }

        public void OnClick(object? sender, CellClickEventArgs args)
        {
            int row = args.Cell.Row;
            int col = args.Cell.Column;

            if (IsPrime(_engine.GetCellValue(row, col)) && !blacklist.Contains((row, col)))
            {
                _engine.SetCellValue(row, col, 1);
                _triangleRenderer.UpdateCell(row, col);
                GetNeighborValues(row, col);
                blacklist.Add((row, col));
                _score++;
                UpdateScoreText();
            }
            else if (_engine.GetCellValue(row, col) == 0)
            {
                _engine.SetCellValue(row, col, 1);
                _triangleRenderer.UpdateCell(row, col);
                GetNeighborValues(row, col);
                blacklist.Add((row, col));
            }
            else 
            {
                _engine.SetCellValue(row, col, 99);
                _engine.GetCellValue(row, col);
                _triangleRenderer.UpdateCell(row, col);
                blacklist.Add((row, col));
                notPrimes.Add((row, col));
            }            

            if (!AnyPrimesLeft())
            {
                EndGameMessage.IsVisible = true;
            }
        }

        private int GenerateRandomPrime()
        {
            int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41,
                            43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
            return primes[random.Next(primes.Length)];
        }

        private void GetNeighborValues(int row, int col)
        {
            var neighbors = _triangleHelper.GetNeighbors(row, col);
            notPrimes = new List<(int, int)>();
            foreach (var (nRow, nCol) in neighbors)
            {
                if (!blacklist.Contains((nRow, nCol)) && _engine.GetCellValue(nRow, nCol) == 0)
                {
                    _engine.SetCellValue(nRow, nCol, random.Next(2, 97));
                    var primeNeighbor = neighbors[random.Next(0, neighbors.Count)];

                    if (_engine.GetCellValue(primeNeighbor.row, primeNeighbor.col) == 0)
                    {
                        _engine.SetCellValue(primeNeighbor.row, primeNeighbor.col, GenerateRandomPrime());
                        _triangleRenderer.UpdateCell(primeNeighbor.row, primeNeighbor.col);
                    }

                    _triangleRenderer.UpdateCell(nRow, nCol);
                }
            }
        }

        // private bool AllPrimes()
        // {
        //     List<int> cellValues = new List<int>();
        //     for (int i = 0; i < _engine.Rows; i++)
        //     {
        //         for (int j = 0; j < _engine.Columns; j++)
        //         {
        //             int cellValue = _engine.GetCellValue(i, j);
        //             cellValues.Add(cellValue);
        //         }
        //     }

        //     if (cellValues.Any(v => IsPrime(v)) && cellValues.Any(v => v != 0) && cellValues.Any(v => v != 99))
        //         return true;
        //     else
        //         return false;
        // }

        private bool AnyPrimesLeft()
        {
            for (int i = 0; i < _engine.Rows; i++)
            {
                for (int j = 0; j < _engine.Columns; j++)
                {
                    int value = _engine.GetCellValue(i, j);
                    if (IsPrime(value))
                        return true;
                }
            }
            return false;
        }
    }
}