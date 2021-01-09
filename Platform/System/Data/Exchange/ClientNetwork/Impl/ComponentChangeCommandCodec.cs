namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;

    public class ComponentChangeCommandCodec : Codec
    {
        private Protocol protocol;
        private Codec entityCodec;
        private Codec longCodec;

        public object Decode(ProtocolBuffer protocolBuffer)
        {
            ComponentChangeCommand instance = Activator.CreateInstance<ComponentChangeCommand>();
            this.DecodeToInstance(protocolBuffer, instance);
            return instance;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance)
        {
            ComponentChangeCommand command = (ComponentChangeCommand) instance;
            EntityInternal internal2 = (EntityInternal) this.entityCodec.Decode(protocolBuffer);
            long uid = (long) this.longCodec.Decode(protocolBuffer);
            Type typeByUid = this.protocol.GetTypeByUid(uid);
            Component component = null;
            component = !internal2.HasComponent(typeByUid) ? ((Component) Activator.CreateInstance(typeByUid)) : internal2.GetComponent(typeByUid);
            this.protocol.GetCodec(typeByUid).DecodeToInstance(protocolBuffer, component);
            command.Entity = internal2;
            command.Component = component;
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            ComponentChangeCommand command = (ComponentChangeCommand) data;
            this.entityCodec.Encode(protocolBuffer, command.Entity);
            this.EncodeVaried(protocolBuffer, command.Component);
        }

        private void EncodeVaried(ProtocolBuffer protocolBuffer, object data)
        {
            Type cl = data.GetType();
            long uidByType = this.protocol.GetUidByType(cl);
            protocolBuffer.Writer.Write(uidByType);
            ProtocolBuffer buffer = this.protocol.NewProtocolBuffer();
            this.protocol.GetCodec(cl).Encode(buffer, data);
            this.protocol.WrapPacket(buffer, protocolBuffer.Data);
            this.protocol.FreeProtocolBuffer(buffer);
        }

        public void Init(Protocol protocol)
        {
            this.entityCodec = protocol.GetCodec(typeof(EntityInternal));
            this.longCodec = protocol.GetCodec(typeof(long));
            this.protocol = protocol;
        }
    }
}

