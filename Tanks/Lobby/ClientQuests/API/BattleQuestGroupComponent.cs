namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class BattleQuestGroupComponent : GroupComponent
    {
        public BattleQuestGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public BattleQuestGroupComponent(long key) : base(key)
        {
        }
    }
}

