namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using log4net;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class TcpSocketImpl : PlatformSocket
    {
        private const AddressFamily ADDRESS_FAMILY = AddressFamily.InterNetwork;
        private Socket socket;
        [CompilerGenerated]
        private static Func<IPAddress, bool> <>f__am$cache0;

        public void ConnectAsync(string host, int port, Action completeCallback)
        {
            <ConnectAsync>c__AnonStorey0 storey = new <ConnectAsync>c__AnonStorey0 {
                completeCallback = completeCallback,
                $this = this
            };
            if ((this.socket != null) && this.socket.Connected)
            {
                throw new Exception("Connection in progress");
            }
            IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = addr => addr.AddressFamily == AddressFamily.InterNetwork;
            }
            IPAddress address = hostAddresses.FirstOrDefault<IPAddress>(<>f__am$cache0);
            if (address == null)
            {
                this.LogUnresolvableAddress(host, hostAddresses);
                throw new Exception("Unresolvable address");
            }
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs e = new SocketAsyncEventArgs {
                RemoteEndPoint = new IPEndPoint(address, port)
            };
            e.Completed += new EventHandler<SocketAsyncEventArgs>(storey.<>m__0);
            this.socket.ConnectAsync(e);
        }

        public void Disconnect()
        {
            this.socket.Close();
            this.socket = null;
        }

        private void LogUnresolvableAddress(string host, IPAddress[] addressList)
        {
            ILog logger = LoggerProvider.GetLogger(this);
            logger.ErrorFormat("Couldn't resolve host address {0} as {1} family", host, AddressFamily.InterNetwork);
            logger.ErrorFormat("Available options (Count = {0}):", addressList.Length);
            foreach (IPAddress address in addressList)
            {
                logger.ErrorFormat("{0}, address familiy - {1}", address, address.AddressFamily);
            }
        }

        public int Read(byte[] buffer, int offset, int count) => 
            this.socket.Receive(buffer, offset, count, SocketFlags.None);

        public void Write(byte[] buffer, int offset, int count)
        {
            this.socket.Send(buffer, offset, count, SocketFlags.None);
        }

        public bool IsConnected =>
            (this.socket != null) && this.socket.Connected;

        public int AvailableLength =>
            (this.socket == null) ? 0 : this.socket.Available;

        public bool CanRead =>
            this.socket.Poll(0, SelectMode.SelectRead);

        public bool CanWrite =>
            this.socket.Poll(0, SelectMode.SelectWrite);

        [CompilerGenerated]
        private sealed class <ConnectAsync>c__AnonStorey0
        {
            internal Action completeCallback;
            internal TcpSocketImpl $this;

            internal void <>m__0(object sender, SocketAsyncEventArgs e)
            {
                if (e.SocketError != SocketError.Success)
                {
                    this.$this.socket = null;
                }
                this.completeCallback();
            }
        }
    }
}

