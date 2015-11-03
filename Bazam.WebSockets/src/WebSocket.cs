using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bazam.WebSockets
{
    // adapted (and more or less copy-pasted) from Xamlmonkey at https://gist.github.com/xamlmonkey/4737291
    public class WebSocket
    {
        private const int CHUNK_SIZE = 1024;

        private readonly ClientWebSocket _WebSocket;
        private readonly Uri _Uri;
        private readonly CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _CancellationToken;

        private Action<WebSocket> _OnConnected;
        private Action<string, WebSocket> _OnMessage;
        private Action<WebSocket> _OnDisconnected;

        protected WebSocket(string uri)
        {
            _CancellationToken = _CancellationTokenSource.Token;
            _Uri = new Uri(uri);
            _WebSocket = new ClientWebSocket();
            _WebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="uri">The URI of the WebSocket server.</param>
        /// <returns></returns>
        public static WebSocket Create(string uri)
        {
            return new WebSocket(uri);
        }

        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        public WebSocket Connect()
        {
            ConnectAsync();
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        public WebSocket OnConnected(Action<WebSocket> onConnected)
        {
            _OnConnected = onConnected;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        public WebSocket OnDisconnect(Action<WebSocket> onDisconnect)
        {
            _OnDisconnected = onDisconnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        public WebSocket OnMessage(Action<string, WebSocket> onMessage)
        {
            _OnMessage = onMessage;
            return this;
        }

        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        private async void SendMessageAsync(string message)
        {
            if (_WebSocket.State != WebSocketState.Open) {
                throw new Exception("Connection isn't open.");
            }

            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / CHUNK_SIZE);

            for (var i = 0; i < messagesCount; i++) {
                var offset = (CHUNK_SIZE * i);
                var count = CHUNK_SIZE;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length) {
                    count = messageBuffer.Length - offset;
                }

                await _WebSocket.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _CancellationToken);
            }
        }

        private async void ConnectAsync()
        {
            await _WebSocket.ConnectAsync(_Uri, _CancellationToken);
            CallOnConnected();
            Listen();
        }

        private async void Listen()
        {
            var buffer = new byte[CHUNK_SIZE];

            try {
                while (_WebSocket.State == WebSocketState.Open) {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do {
                        result = await _WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _CancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close) {
                            await
                                _WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            CallOnDisconnected();
                        }
                        else {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    CallOnMessage(stringResult);

                }
            }
            catch (Exception) {
                CallOnDisconnected();
            }
            finally {
                _WebSocket.Dispose();
            }
        }

        private void CallOnMessage(StringBuilder stringResult)
        {
            if (_OnMessage != null)
                RunInTask(() => _OnMessage(stringResult.ToString(), this));
        }

        private void CallOnDisconnected()
        {
            if (_OnDisconnected != null)
                RunInTask(() => _OnDisconnected(this));
        }

        private void CallOnConnected()
        {
            if (_OnConnected != null)
                RunInTask(() => _OnConnected(this));
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}