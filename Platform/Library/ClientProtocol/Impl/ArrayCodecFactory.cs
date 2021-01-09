namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class ArrayCodecFactory : CodecFactory
    {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            Type type = codecInfoWithAttrs.Info.type;
            if (!type.IsArray)
            {
                return null;
            }
            Type elementType = type.GetElementType();
            bool optional = false;
            bool varied = false;
            if (codecInfoWithAttrs.IsAttributePresent<ProtocolCollectionAttribute>())
            {
                ProtocolCollectionAttribute attribute = codecInfoWithAttrs.GetAttribute<ProtocolCollectionAttribute>();
                optional = attribute.Optional;
                varied = attribute.Varied;
            }
            return new ArrayCodec(elementType, new CodecInfoWithAttributes(type.GetElementType(), optional, varied));
        }
    }
}

