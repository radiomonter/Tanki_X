namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class BonusRegionLogicalBuilderSystem : ECSSystem
    {
        public static readonly string REGION_CONFIG_PATH = "battle/bonus/region";

        [OnEventFire]
        public void CreateRegionConfigEntity(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> user)
        {
            Entity entity = base.CreateEntity<BonusRegionAssetsTemplate>(REGION_CONFIG_PATH);
        }

        [OnEventFire]
        public void DeleteRegionConfigEntity(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> user, [JoinAll] SingleNode<BonusRegionAssetsComponent> regionAssets)
        {
            base.DeleteEntity(regionAssets.Entity);
        }
    }
}

