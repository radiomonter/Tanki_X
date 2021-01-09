namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class BonusClientConfigLoaderSystem : ECSSystem
    {
        [OnEventFire]
        public void CollectComponentToBonusClientConfigEntity(NodeAddedEvent e, BonusClientConfigLoaderNode bonusConfigLoader, [JoinByBattle] SingleNode<BattleComponent> battle)
        {
            ((GameObject) bonusConfigLoader.resourceData.Data).CollectComponents(battle.Entity);
            base.DeleteEntity(bonusConfigLoader.Entity);
        }

        [OnEventFire]
        public void DeleteBonusClientConfigLoaderWhenBattleRemoved(NodeRemoveEvent e, SingleNode<BonusClientConfigPrefabComponent> battle, [JoinByBattle] BonusClientConfigLoaderForDeleteNode bonusConfigLoader)
        {
            base.DeleteEntity(bonusConfigLoader.Entity);
        }

        [OnEventFire]
        public void LoadBonusClientConfigPrefab(NodeAddedEvent e, BattleNode battle)
        {
            Entity entity = base.CreateEntity("BonusClientConfigPrefabLoader");
            entity.AddComponent<BonusClientConfigPrefabLoaderComponent>();
            entity.AddComponent(new BattleGroupComponent(battle.Entity));
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(battle.bonusClientConfigPrefab.AssetGuid)));
            entity.AddComponent<AssetRequestComponent>();
        }

        [Not(typeof(BonusClientConfigComponent))]
        public class BattleNode : Node
        {
            public BonusClientConfigPrefabComponent bonusClientConfigPrefab;
            public BattleGroupComponent battleGroup;
            public SelfComponent self;
        }

        public class BonusClientConfigLoaderForDeleteNode : Node
        {
            public BonusClientConfigPrefabLoaderComponent bonusClientConfigPrefabLoader;
            public BattleGroupComponent battleGroup;
        }

        public class BonusClientConfigLoaderNode : Node
        {
            public BonusClientConfigPrefabLoaderComponent bonusClientConfigPrefabLoader;
            public BattleGroupComponent battleGroup;
            public ResourceDataComponent resourceData;
        }
    }
}

