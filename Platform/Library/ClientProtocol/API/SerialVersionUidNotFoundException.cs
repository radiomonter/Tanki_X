namespace Platform.Library.ClientProtocol.API
{
    using System;

    public class SerialVersionUidNotFoundException : Exception
    {
        public SerialVersionUidNotFoundException(Type type) : base("Type = " + type)
        {
        }
    }
}

