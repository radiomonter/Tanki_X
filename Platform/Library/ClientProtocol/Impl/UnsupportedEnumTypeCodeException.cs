namespace Platform.Library.ClientProtocol.Impl
{
    using System;

    public class UnsupportedEnumTypeCodeException : Exception
    {
        public UnsupportedEnumTypeCodeException(TypeCode typeCode) : base($"Unsupported enum TypeCode {Enum.GetName(typeof(TypeCode), typeCode)}. Use Byte instead!")
        {
        }
    }
}

