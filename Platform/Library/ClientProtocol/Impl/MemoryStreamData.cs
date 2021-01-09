namespace Platform.Library.ClientProtocol.Impl
{
    using System;

    public class MemoryStreamData : StreamData<MemoryStream>
    {
        public byte[] GetBuffer() => 
            base.CastedStream.GetBuffer();
    }
}

