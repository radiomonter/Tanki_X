namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x8d333b1360efa28L)]
    public class ParentGroupComponent : GroupComponent
    {
        public ParentGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ParentGroupComponent(long key) : base(key)
        {
        }
    }
}

