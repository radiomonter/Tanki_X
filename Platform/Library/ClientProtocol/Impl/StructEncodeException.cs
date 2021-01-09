namespace Platform.Library.ClientProtocol.Impl
{
    using System;
    using System.Reflection;

    public class StructEncodeException : Exception
    {
        public StructEncodeException(Type structType, PropertyInfo prop, Exception e) : base($"structType={structType} prop={prop}", e)
        {
        }
    }
}

