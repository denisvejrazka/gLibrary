using Avalonia.Controls;
using gLibrary.Engine;
using gLibrary.Rendering;
using gLibrary.Helping;
using TicTacToe.Game;
using TicTacToe.Game.Mapping;
using gLibrary.Events;
using gLibrary.Communication;
using Avalonia.Threading;

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
    //private WebSocketManager _webSocketManager;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGrid();
        //InitWebSocket();
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

    // private async void InitWebSocket()
    // {
    //     _webSocketManager = new WebSocketManager();
    //     _webSocketManager.OnMessageReceived += (row, col) =>
    //     {
    //         Dispatcher.UIThread.InvokeAsync(() =>
    //         {
    //             _gameController.MakeMove(row, col);
    //             _squareRenderer.UpdateCell(row, col);
    //         });
    //     };

    //     await _webSocketManager.InitializeAsync(""); //server IP add
    // }

    public void OnClick(object? sender, CellClickEventArgs args)
    {
        int row = args.Cell.Row;
        int col = args.Cell.Column;
        // if (_webSocketManager?.IsConnected == true)
        // {
        //     _webSocketManager.Send(row, col);
        // }
        _gameController.MakeMove(row, col);
        _renderer.RenderGrid(_engine, _mapper, _size);
    }
}