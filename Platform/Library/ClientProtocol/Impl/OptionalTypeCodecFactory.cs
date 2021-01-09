namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    public class OptionalTypeCodecFactory : CodecFactory
    {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            Type type = codecInfoWithAttrs.Info.type;
            return ((!type.IsGenericType || !ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Optional<>))) ? null : new OptionalTypeCodec(type, new CodecInfoWithAttributes(type.GetGenericArguments()[0], false, codecInfoWithAttrs.Info.varied)));
        }
    }
}

