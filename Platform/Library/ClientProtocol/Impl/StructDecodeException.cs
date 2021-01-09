namespace Platform.Library.ClientProtocol.Impl
{
    using System;

    public class StructDecodeException : Exception
    {
        public StructDecodeException(object structInstance, Exception e) : base($"partial struct={structInstance}", e)
        {
        }
    }
}

