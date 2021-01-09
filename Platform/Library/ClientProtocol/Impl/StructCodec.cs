namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class StructCodec : Codec
    {
        private int codecCount;
        private readonly Type type;
        private readonly List<PropertyRequest> requests;
        private List<PropertyCodec> codecs;

        public StructCodec(Type type, List<PropertyRequest> requests)
        {
            this.type = type;
            this.requests = requests;
        }

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            object instance = Activator.CreateInstance(this.type);
            this.DecodeToInstance(protocolBuffer, instance);
            return instance;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            int num = 0;
            try
            {
                while (num < this.codecCount)
                {
                    PropertyCodec codec = this.codecs[num];
                    object obj2 = codec.Codec.Decode(protocolBuffer);
                    codec.PropertyInfo.SetValue(instance, obj2, null);
                    num++;
                }
            }
            catch (Exception exception)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i <= num; i++)
                {
                    PropertyCodec codec2 = this.codecs[i];
                    builder.Append(codec2.PropertyInfo.Name);
                    builder.Append("=");
                    builder.Append(codec2.PropertyInfo.GetValue(instance, BindingFlags.Default, null, null, null));
                    builder.Append("\n");
                }
                object[] objArray1 = new object[] { "Struct decode failed; Type: ", instance.GetType().Name, " decodedPropertis: ", builder };
                throw new Exception(string.Concat(objArray1), exception);
            }
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            for (int i = 0; i < this.codecCount; i++)
            {
                PropertyCodec codec = this.codecs[i];
                try
                {
                    object obj2 = codec.PropertyInfo.GetValue(data, BindingFlags.Default, null, null, null);
                    codec.Codec.Encode(protocolBuffer, obj2);
                }
                catch (Exception exception)
                {
                    object[] objArray1 = new object[] { "Property encoding exception, property=", codec.PropertyInfo.Name, " type=", codec.PropertyInfo.DeclaringType };
                    throw new Exception(string.Concat(objArray1), exception);
                }
            }
        }

        public void Init(Protocol protocol)
        {
            this.codecCount = this.requests.Count;
            this.codecs = new List<PropertyCodec>(this.codecCount);
            for (int i = 0; i < this.codecCount; i++)
            {
                PropertyRequest request = this.requests[i];
                Codec codec = protocol.GetCodec(request.CodecInfoWithAttributes);
                this.codecs.Add(new PropertyCodec(codec, request.PropertyInfo));
            }
        }

        public override string ToString() => 
            "StructCodec[" + this.type + "]";
    }
}

