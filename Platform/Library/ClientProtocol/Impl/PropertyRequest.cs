namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class PropertyRequest
    {
        public PropertyRequest(System.Reflection.PropertyInfo propertyInfo, Platform.Library.ClientProtocol.API.CodecInfoWithAttributes codecInfoWithAttributes)
        {
            this.PropertyInfo = propertyInfo;
            this.CodecInfoWithAttributes = codecInfoWithAttributes;
        }

        public System.Reflection.PropertyInfo PropertyInfo { get; private set; }

        public Platform.Library.ClientProtocol.API.CodecInfoWithAttributes CodecInfoWithAttributes { get; private set; }
    }
}

