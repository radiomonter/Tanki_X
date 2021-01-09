namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    public class EnumCodecFactory : CodecFactory
    {
        private readonly Dictionary<Type, EnumCodec> codecs = new Dictionary<Type, EnumCodec>();

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs)
        {
            EnumCodec codec;
            Type key = codecInfoWithAttrs.Info.type;
            if (!key.IsEnum)
            {
                return null;
            }
            if (!this.codecs.TryGetValue(key, out codec))
            {
                codec = new EnumCodec(key);
                this.codecs[key] = codec;
            }
            return codec;
        }
    }
}

