namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x76e0f9828c6734dfL)]
    public class BonusRegionGroupComponent : GroupComponent
    {
        public BonusRegionGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public BonusRegionGroupComponent(long key) : base(key)
        {
        }
    }
}

