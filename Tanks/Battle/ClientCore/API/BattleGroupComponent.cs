namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0xfd445b4ede9ca9cL)]
    public class BattleGroupComponent : GroupComponent
    {
        public BattleGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public BattleGroupComponent(long key) : base(key)
        {
        }
    }
}

