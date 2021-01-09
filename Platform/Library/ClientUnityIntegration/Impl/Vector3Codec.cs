namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.IO;
    using UnityEngine;

    public class Vector3Codec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            BinaryReader reader = protocolBuffer.Reader;
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            Vector3 vector = (Vector3) data;
            BinaryWriter writer = protocolBuffer.Writer;
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }

        public override void Init(Protocol protocol)
        {
        }
    }
}

