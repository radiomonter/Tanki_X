namespace Platform.Library.ClientProtocol.API
{
    using System;

    public class SerializationUidUtils
    {
        public static long GetUid(Type type)
        {
            if (!Attribute.IsDefined(type, typeof(SerialVersionUIDAttribute)))
            {
                throw new SerialVersionUidNotFoundException(type);
            }
            return ((SerialVersionUIDAttribute) Attribute.GetCustomAttribute(type, typeof(SerialVersionUIDAttribute))).value;
        }

        public static bool HasSelfUid(Type type) => 
            Attribute.IsDefined(type, typeof(SerialVersionUIDAttribute), false);
    }
}

