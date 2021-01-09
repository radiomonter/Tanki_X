namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public interface PlatformSocket
    {
        void ConnectAsync(string host, int port, Action completeCallback);
        void Disconnect();
        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);

        bool IsConnected { get; }

        int AvailableLength { get; }

        bool CanRead { get; }

        bool CanWrite { get; }
    }
}

