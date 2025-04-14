using System;
using System;
using Websocket.Client;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace gLibrary.Communication
{
    public class WebSocketManager
    {
        private WebsocketClient _client;

        public bool IsConnected => _client?.IsRunning ?? false;

        public event Action<int, int>? OnMessageReceived;

        public async Task InitializeAsync(string uri)
        {
            var url = new Uri(uri);
            _client = new WebsocketClient(url);

            _client.MessageReceived
                .Where(msg => msg.Text != null)
                .Subscribe(msg =>
                {
                    var parts = msg.Text.Split(',');
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0], out int row) &&
                        int.TryParse(parts[1], out int col))
                    {
                        OnMessageReceived?.Invoke(row, col);
                    }
                });

            await _client.Start();
        }

        public void Send(int row, int col)
        {
            if (IsConnected)
            {
                _client.Send($"{row},{col}");
            }
        }
    }
}