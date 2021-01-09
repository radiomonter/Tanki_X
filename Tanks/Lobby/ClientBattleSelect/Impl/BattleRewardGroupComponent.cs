namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x1606e3669a9L)]
    public class BattleRewardGroupComponent : GroupComponent
    {
        public BattleRewardGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public BattleRewardGroupComponent(long key) : base(key)
        {
        }
    }
}

