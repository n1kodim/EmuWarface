namespace EmuWarface.Server.Network
{
	public class SocketOptions
	{
		public int AcceptorBacklog { get; set; } = 1024;
		public bool DualMode { get; set; }
		public bool KeepAlive { get; set; }
		public int TcpKeepAliveTime { get; set; } = -1;
		public int TcpKeepAliveInterval { get; set; } = -1;
		public int TcpKeepAliveRetryCount { get; set; } = -1;
		public bool NoDelay { get; set; }
		public bool ReuseAddress { get; set; }
		public bool ExclusiveAddressUse { get; set; }
		public int ReceiveBufferSize { get; set; } = 8192;
		public int ReceiveBufferLimit { get; set; } = 0;
		public int SendBufferSize { get; set; } = 8192;
        public int SendBufferLimit { get; set; } = 0;
	}
}