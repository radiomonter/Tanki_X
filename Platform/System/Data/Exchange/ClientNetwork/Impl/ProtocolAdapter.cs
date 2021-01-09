namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Library.ClientProtocol.Impl;
    using System;
    using System.Runtime.InteropServices;

    public interface ProtocolAdapter
    {
        void AddChunk(byte[] chunk, int length);
        MemoryStreamData Encode(CommandPacket packet);
        void FinalizeDecodedCommandPacket(CommandPacket commandPacket);
        bool TryDecode(out CommandPacket packet);

        bool SplitShareCommand { get; set; }
    }
}

