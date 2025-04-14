using Avalonia.Controls;
using gLibrary.Engine;
using gLibrary.Rendering;
using gLibrary.Helping;
using TicTacToe.Game;
using TicTacToe.Game.Mapping;
using gLibrary.Events;

namespace TicTacToe.Views;

public partial class MainWindow : Window
{
    private GridEngine _engine;
    private Renderer _renderer;
    private SquareHelper _squareHelper;
    private SquareRenderer _squareRenderer;
    private TicTacToeMapper _mapper;
    private GameController _gameController;
    private int _size;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        _size = 55;
        _engine = new GridEngine(3, 3);
        _engine.GenerateGrid();
        _mapper = new TicTacToeMapper();
        _squareHelper = new SquareHelper(_engine);
        _squareRenderer = new SquareRenderer(TicTacToeBackground, _size, 0.5, _squareHelper, _engine, _mapper, OnClick);
        _renderer = new Renderer(_squareRenderer);
        _renderer.RenderGrid(_engine, _mapper, _size);

        _gameController = new GameController(_engine);
    }

    public void OnClick(object? sender, CellClickEventArgs args)
    {
        int row = args.Cell.Row;
        int col = args.Cell.Column;
        _gameController.MakeMove(row, col);
        _renderer.RenderGrid(_engine, _mapper, _size);
    }
}