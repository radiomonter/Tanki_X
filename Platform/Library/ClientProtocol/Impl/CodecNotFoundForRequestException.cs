namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class CodecNotFoundForRequestException : Exception
    {
        public CodecNotFoundForRequestException(CodecInfo request) : base("request = " + request)
        {
        }
    }
}

