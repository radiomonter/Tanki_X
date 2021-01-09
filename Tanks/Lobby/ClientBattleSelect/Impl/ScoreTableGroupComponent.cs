namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ScoreTableGroupComponent : GroupComponent
    {
        public ScoreTableGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ScoreTableGroupComponent(long key) : base(key)
        {
        }
    }
}

