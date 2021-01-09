namespace Platform.Library.ClientProtocol.API
{
    using System;

    public static class BufferUtils
    {
        public static byte[] GetBufferWithValidSize(byte[] input, int size)
        {
            int length = input.Length;
            return ((size <= length) ? input : new byte[size]);
        }
    }
}

