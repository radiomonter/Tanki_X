namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x159ceac9993L), Shared]
    public class UnitGroupComponent : GroupComponent
    {
        public UnitGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public UnitGroupComponent(long key) : base(key)
        {
        }
    }
}

