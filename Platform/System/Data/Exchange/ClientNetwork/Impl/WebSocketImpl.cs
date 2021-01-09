namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.IO;

    public class WebSocketImpl : PlatformSocket
    {
        private WebSocket socket;
        private Action completeCallback;

        public void ConnectAsync(string host, int port, Action completeCallback)
        {
            if ((this.socket != null) && this.socket.IsConnected)
            {
                throw new Exception("Connection in progress");
            }
            this.completeCallback = completeCallback;
            this.socket = new WebSocket();
            this.socket.ConnectAsync($"ws://{host}:{port}/websocket", new Action(this.OnComplete));
        }

        public void Disconnect()
        {
            this.socket.Close();
            this.socket = null;
        }

        private void OnComplete()
        {
            if (!this.socket.IsConnected)
            {
                this.socket = null;
            }
            this.completeCallback();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int num = this.socket.Receive(buffer);
            if (this.socket.GetError() != null)
            {
                throw new IOException(this.socket.GetError());
            }
            return num;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            byte[] dst = new byte[count];
            Buffer.BlockCopy(buffer, 0, dst, 0, count);
            this.socket.Send(dst);
            if (this.socket.GetError() != null)
            {
                throw new IOException(this.socket.GetError());
            }
        }

        public bool IsConnected =>
            (this.socket != null) && this.socket.IsConnected;

        public int AvailableLength =>
            (this.socket == null) ? 0 : this.socket.AvailableLength;

        public bool CanRead =>
            this.AvailableLength > 0;

        public bool CanWrite =>
            this.IsConnected;
    }
}

