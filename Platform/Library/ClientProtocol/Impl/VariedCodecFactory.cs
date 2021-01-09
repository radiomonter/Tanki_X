namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class VariedCodecFactory : CodecFactory
    {
        private VariedStructCodec structCodec = new VariedStructCodec();
        private VariedTypeCodec typeCodec = new VariedTypeCodec();

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) => 
            !codecInfoWithAttrs.Info.varied ? null : (!ReferenceEquals(codecInfoWithAttrs.Info.type, typeof(Type)) ? ((Codec) this.structCodec) : ((Codec) this.typeCodec));
    }
}

