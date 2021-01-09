namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using Platform.System.Data.Exchange.ClientNetwork.Impl;
    using System;
    using System.Runtime.InteropServices;

    public interface NetworkService
    {
        event Action<Command, Exception> OnCommandExecuteException;

        void ConnectAsync(string host, int port, Action completeCallback);
        void Disconnect();
        void ReadAndExecuteCommands(long networkSliceTime = 0L, long networkMaxDelayTime = 0L);
        void SendCommandPacket(CommandPacket packet);
        void WriteCommands();

        bool IsConnected { get; }

        bool IsDecodeState { get; }

        bool SkipThrowOnCommandExecuteException { get; set; }

        bool SplitShareCommand { set; }
    }
}

