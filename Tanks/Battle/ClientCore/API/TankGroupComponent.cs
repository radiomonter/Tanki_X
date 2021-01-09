namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x38bb994106b9f97fL)]
    public class TankGroupComponent : GroupComponent
    {
        public TankGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public TankGroupComponent(long key) : base(key)
        {
        }
    }
}

