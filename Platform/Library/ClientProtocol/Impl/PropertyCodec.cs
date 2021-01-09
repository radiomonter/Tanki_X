namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class PropertyCodec
    {
        public PropertyCodec(Platform.Library.ClientProtocol.API.Codec codec, System.Reflection.PropertyInfo propertyInfo)
        {
            this.Codec = codec;
            this.PropertyInfo = propertyInfo;
        }

        public Platform.Library.ClientProtocol.API.Codec Codec { get; private set; }

        public System.Reflection.PropertyInfo PropertyInfo { get; private set; }
    }
}

