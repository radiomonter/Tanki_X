namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EntityCodec : NotOptionalCodec
    {
        private Codec longCodec;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            EntityInternal internal2;
            long entityId = (long) this.longCodec.Decode(protocolBuffer);
            return (!SharedEntityRegistry.TryGetEntity(entityId, out internal2) ? SharedEntityRegistry.CreateEntity(entityId) : internal2);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            this.longCodec.Encode(protocolBuffer, ((Entity) data).Id);
        }

        public override void Init(Protocol protocol)
        {
            this.longCodec = protocol.GetCodec(typeof(long));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.SharedEntityRegistry SharedEntityRegistry { get; set; }
    }
}

