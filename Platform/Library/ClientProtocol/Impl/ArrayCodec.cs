namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Text;

    public class ArrayCodec : Codec
    {
        private readonly Type elementType;
        private readonly CodecInfoWithAttributes elementCodecInfo;
        private Codec elementCodec;

        public ArrayCodec(Type elementType, CodecInfoWithAttributes elementCodecInfo)
        {
            this.elementType = elementType;
            this.elementCodecInfo = elementCodecInfo;
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            object obj3;
            int index = 0;
            Array array = null;
            int length = 0;
            try
            {
                length = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
                array = Array.CreateInstance(this.elementType, length);
                while (true)
                {
                    if (index >= length)
                    {
                        obj3 = array;
                        break;
                    }
                    object obj2 = this.elementCodec.Decode(protocolBuffer);
                    array.SetValue(obj2, index);
                    index++;
                }
            }
            catch (Exception exception)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i <= index; i++)
                {
                    object obj4 = array.GetValue(i);
                    builder.Append(i);
                    builder.Append(") ");
                    builder.Append(obj4);
                    builder.Append("\n");
                }
                object[] objArray1 = new object[] { "Array decode failed; ElementType: ", this.elementType.Name, " length: ", length, " decodedElements: ", builder };
                throw new Exception(string.Concat(objArray1), exception);
            }
            return obj3;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            throw new NotImplementedException();
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            Array array = (Array) data;
            int length = array.Length;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, length);
            for (int i = 0; i < length; i++)
            {
                this.elementCodec.Encode(protocolBuffer, array.GetValue(i));
            }
        }

        public void Init(Protocol protocol)
        {
            this.elementCodec = protocol.GetCodec(this.elementCodecInfo);
        }
    }
}

