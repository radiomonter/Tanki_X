namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    public class DictionaryCodecFactory : CodecFactory
    {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            Type type = codecInfoWithAttrs.Info.type;
            if (!type.IsGenericType || !ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Dictionary<,>)))
            {
                return null;
            }
            Type type2 = type.GetGenericArguments()[0];
            Type type3 = type.GetGenericArguments()[1];
            bool optional = false;
            bool varied = false;
            bool optionalValue = false;
            bool variedValue = false;
            if (codecInfoWithAttrs.IsAttributePresent<ProtocolDictionaryAttribute>())
            {
                ProtocolDictionaryAttribute attribute = codecInfoWithAttrs.GetAttribute<ProtocolDictionaryAttribute>();
                optional = attribute.OptionalKey;
                varied = attribute.VariedKey;
                optionalValue = attribute.OptionalValue;
                variedValue = attribute.VariedValue;
            }
            return new DictionaryCodec(type, new CodecInfoWithAttributes(type2, optional, varied), new CodecInfoWithAttributes(type3, optionalValue, variedValue));
        }
    }
}

