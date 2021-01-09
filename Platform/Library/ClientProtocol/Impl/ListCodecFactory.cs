namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    public class ListCodecFactory : CodecFactory
    {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            Type type = codecInfoWithAttrs.Info.type;
            if (!type.IsGenericType || !ReferenceEquals(type.GetGenericTypeDefinition(), typeof(List<>)))
            {
                return null;
            }
            Type type2 = type.GetGenericArguments()[0];
            bool optional = false;
            bool varied = false;
            if (codecInfoWithAttrs.IsAttributePresent<ProtocolCollectionAttribute>())
            {
                ProtocolCollectionAttribute attribute = codecInfoWithAttrs.GetAttribute<ProtocolCollectionAttribute>();
                optional = attribute.Optional;
                varied = attribute.Varied;
            }
            return new ListCodec(type, new CodecInfoWithAttributes(type2, optional, varied));
        }
    }
}

