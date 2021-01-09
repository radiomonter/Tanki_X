namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GroupComponentCodec : NotOptionalCodec
    {
        private Codec longCodec;
        private Protocol protocol;

        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            long uid = (long) this.longCodec.Decode(protocolBuffer);
            Type typeByUid = this.protocol.GetTypeByUid(uid);
            return GroupRegistry.FindOrCreateGroup(typeByUid, (long) this.longCodec.Decode(protocolBuffer));
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            GroupComponent component = (GroupComponent) data;
            long uidByType = this.protocol.GetUidByType(component.GetType());
            this.longCodec.Encode(protocolBuffer, uidByType);
            this.longCodec.Encode(protocolBuffer, component.Key);
        }

        public override void Init(Protocol protocol)
        {
            this.longCodec = protocol.GetCodec(typeof(long));
            this.protocol = protocol;
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.GroupRegistry GroupRegistry { get; set; }
    }
}

