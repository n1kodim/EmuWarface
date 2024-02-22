using System.Net;
using System.Net.Sockets;

namespace EmuWarface.Server.Network
{
	public abstract class SocketListener<T> : IDisposable where T : NetworkSession, new()
    {
        public string Address { private set; get; }
        public int Port { private set; get; }
        public bool IsStarted { get; set; }
        public bool IsAccepting { get; set; }

        protected abstract void OnSessionConnected(T session);
        protected abstract void OnError(SocketError error);

        protected readonly object _lock = new object();
        protected List<T> _sessions;

        private Socket _socket;
        private SocketOptions _options;
        private SocketAsyncEventArgs _eventArg;

        protected SocketListener()
		{
			_sessions = new List<T>();
            _options = new SocketOptions();
            _eventArg = new SocketAsyncEventArgs();
            _eventArg.Completed += OnAsyncCompleted;
        }

		public bool Start(string host, int port)
		{
			if (IsStarted)
				return false;

            var endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Address = host;
            Port = port;

            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _options.ReuseAddress);
			_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, _options.ExclusiveAddressUse);
			
			if (_socket.AddressFamily == AddressFamily.InterNetworkV6)
				_socket.DualMode = _options.DualMode;

			_socket.Bind(endPoint);
			_socket.Listen(_options.AcceptorBacklog);

			IsStarted = true;
			IsAccepting = true;

			StartAccept(_eventArg);
			return true;
		}
		
		public bool Stop()
		{
			if (!IsStarted)
				return false;
				
			IsAccepting = false;
			_eventArg.Completed -= OnAsyncCompleted;
			
			_socket?.Close();
			_socket?.Dispose();
			_eventArg?.Dispose();
			
			DisconnectAllAsync().GetAwaiter().GetResult();
			IsStarted = false;

			return true;
		}
		
		private void StartAccept(SocketAsyncEventArgs e)
		{
			e.AcceptSocket = null;

			if (!_socket.AcceptAsync(e))
				ProcessAccept(e);
		}
		
		private void ProcessAccept(SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				var session = new T();
				session.OnAccept(e.AcceptSocket);
				lock (_lock)
				{
                    _sessions.Add(session);
                }
				OnSessionConnected(session);

            }
			else
				SendError(e.SocketError);

			if (IsAccepting)
				StartAccept(e);
		}
		
		private void OnAsyncCompleted(object? sender, SocketAsyncEventArgs e)
		{
			if (IsDisposed)
				return;

			ProcessAccept(e);
		}

		public async Task<bool> DisconnectAllAsync()
		{
			if (!IsStarted)
				return false;

			foreach (var session in _sessions)
			{
                await session.DisconnectAsync();
                lock (_lock)
                {
                    _sessions.Remove(session);
                }
            }

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
		
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual async void Dispose(bool disposingManagedResources)
		{
			if (!IsDisposed)
			{
				if (disposingManagedResources)
					Stop();

				IsDisposed = true;
			}
		}
	}
}