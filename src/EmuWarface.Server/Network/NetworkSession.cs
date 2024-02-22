using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using XmppDotNet.Xml.Parser;

namespace EmuWarface.Server.Network
{
    public abstract class NetworkSession : IDisposable
    {
        public readonly Guid Guid;
        public string RemoteAddress { get; private set; }
        public int RemotePort { get; private set; }
            
        public NetworkSession()
        {
            Guid = Guid.NewGuid();
            _options = new SocketOptions();
            _streamParser = new StreamParser();
            _cts = new CancellationTokenSource();
        }

        private SocketOptions _options;
        private Socket _socket;
        private Stream _stream;
        private bool _isHandshaked;

        protected CancellationTokenSource _cts;
        protected readonly StreamParser _streamParser;
        protected abstract void OnDisconnected();
        protected abstract void OnError(SocketError error);

        public void OnAccept(Socket socket)
        {
            _socket = socket;

            var endPoint = _socket.RemoteEndPoint as IPEndPoint;
            RemoteAddress = endPoint.Address.ToString();
            RemotePort = endPoint.Port;

            if (_options.KeepAlive)
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            if (_options.TcpKeepAliveTime >= 0)
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _options.TcpKeepAliveTime);
            if (_options.TcpKeepAliveInterval >= 0)
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _options.TcpKeepAliveInterval);
            if (_options.TcpKeepAliveRetryCount >= 0)
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _options.TcpKeepAliveRetryCount);
            if (_options.NoDelay)
                _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

            _stream = new NetworkStream(socket);
            StartReceiverTask();
        }

        public async Task<long> SendAsync(byte[] buffer)
        {
            if (IsDisposed || buffer.Length == 0)
                return 0;

            try
            {
                await _stream.WriteAsync(buffer, 0, buffer.Length);
                return buffer.Length;
            }
            catch
            {
                SendError(SocketError.OperationAborted);
                await DisconnectAsync();
                return 0;
            }
        }

        protected void StartReceiverTask()
        {
            _ = Task.Run(this.ReceiveAsync);
        }

        private async Task ReceiveAsync()
        {
            byte[] buffer = new byte[_options.ReceiveBufferSize];

            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    int received = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (received == 0)
                    {
                        SendError(SocketError.OperationAborted);
                        await DisconnectAsync();
                    }

                    _streamParser.Write(buffer, 0, received);
                }
            }
            catch
            {
                SendError(SocketError.OperationAborted);
                await DisconnectAsync();
                return;
            }
        }

        protected async Task<bool> HandshakeAsync()
        {
            if(_isHandshaked)
            {
                SendError(SocketError.OperationAborted);
                await DisconnectAsync();
                return false;
            }

            SslStream sslStream = new SslStream(_stream, false, (sender, cert, chain, err) => true);
            _stream = sslStream;

            try
            {
                await sslStream.AuthenticateAsServerAsync(Globals.Certificate, true, SslProtocols.Tls, false);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                SendError(SocketError.NotConnected);
                await DisconnectAsync();
                return false;
            }

            _isHandshaked = true;
            return true;
        }

        private void SendError(SocketError error)
        {
            if ((error == SocketError.ConnectionAborted) ||
                (error == SocketError.ConnectionRefused) ||
                (error == SocketError.ConnectionReset) ||
                (error == SocketError.OperationAborted) ||
                (error == SocketError.Shutdown))
                return;

            OnError(error);
        }

        public async Task DisconnectAsync()
        {
            await Dispose(true);
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Dispose(true).GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }

        protected virtual async Task<bool> Dispose(bool disposingManagedResources)
        {
            if (IsDisposed)
                return false;

            IsDisposed = true;

            if (!disposingManagedResources)
                return true;

            try
            {
                try
                {
                    if (_stream is SslStream stream)
                        await stream.ShutdownAsync();
                }
                catch (Exception) { }

                await _stream.DisposeAsync();

                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException) { }

                _socket.Close();
                _socket.Dispose();
            }
            catch (ObjectDisposedException) { }
            finally { OnDisconnected(); }

            return true;
        }
    }
}
