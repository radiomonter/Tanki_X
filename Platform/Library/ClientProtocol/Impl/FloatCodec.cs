namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    public class FloatCodec : NotOptionalCodec
    {
        public static int ENCODE_NAN_ERRORS;
        public static int DECODE_NAN_ERRORS;
        public static Exception encodeErrorStack;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            float f = protocolBuffer.Reader.ReadSingle();
            if (float.IsNaN(f) || float.IsInfinity(f))
            {
                DECODE_NAN_ERRORS++;
            }
            return f;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            float f = (float) data;
            if (float.IsNaN(f) || float.IsInfinity(f))
            {
                ENCODE_NAN_ERRORS++;
                encodeErrorStack = new Exception();
            }
            protocolBuffer.Writer.Write(f);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

