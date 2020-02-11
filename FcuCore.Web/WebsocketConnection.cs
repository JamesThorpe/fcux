using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcuCore.Communications;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FcuCore.Web {
    internal class WebsocketConnection
    {
        private WebSocket _webSocket;
        private CbusManager _manager;

        public async Task HandleConnection(HttpContext context, WebSocket webSocket)
        {
            _webSocket = webSocket;

            //TODO: add keepalive and timeouts

            _manager = (CbusManager)context.RequestServices.GetService(typeof(CbusManager));
            if (_manager.ConnectionState == CbusConnectionState.Connected) {
                var msgs = _manager.Messenger;
                if (msgs != null) {
                    msgs.MessageReceived += CbusMessageReceived;
                }
            }

            _manager.ConnectionStateChanged += ConnectionStateChanged;

            await SendCbusConnectionStatus();

            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue) {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            _manager.ConnectionStateChanged -= ConnectionStateChanged;
            if (_manager.ConnectionState == CbusConnectionState.Connected) {
                _manager.Messenger.MessageReceived -= CbusMessageReceived;
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private void ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs args)
        {
            if (args.ConnectionState == CbusConnectionState.Connected) {
                _manager.Messenger.MessageReceived += CbusMessageReceived;
            } else {
                _manager.Messenger.MessageReceived -= CbusMessageReceived;
            }

            SendCbusConnectionStatus();

        }

        private async void CbusMessageReceived(object sender, CbusMessageEventArgs args)
        {
            await SendMessage(new {Type = "cbus", Message = args.Message});
        }

        private async Task SendCbusConnectionStatus()
        {
            await SendMessage(new {Type = "connection-status", IsConnected = _manager.ConnectionState == CbusConnectionState.Connected});
        }

        private async Task SendMessage(object msg)
        {
            var s = JsonConvert.SerializeObject(msg);
            await _webSocket.SendAsync(Encoding.UTF8.GetBytes(s), WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}