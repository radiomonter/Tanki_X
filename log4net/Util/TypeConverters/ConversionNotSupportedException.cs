namespace log4net.Util.TypeConverters
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConversionNotSupportedException : ApplicationException
    {
        public ConversionNotSupportedException()
        {
        }

        public ConversionNotSupportedException(string message) : base(message)
        {
        }

        protected ConversionNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConversionNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static ConversionNotSupportedException Create(Type destinationType, object sourceValue) => 
            Create(destinationType, sourceValue, null);

        public static ConversionNotSupportedException Create(Type destinationType, object sourceValue, Exception innerException)
        {
            if (sourceValue == null)
            {
                return new ConversionNotSupportedException("Cannot convert value [null] to type [" + destinationType + "]", innerException);
            }
            object[] objArray1 = new object[] { "Cannot convert from type [", sourceValue.GetType(), "] value [", sourceValue, "] to type [", destinationType, "]" };
            return new ConversionNotSupportedException(string.Concat(objArray1), innerException);
        }
    }
}

