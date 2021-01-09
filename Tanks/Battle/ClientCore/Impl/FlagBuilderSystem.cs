namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class FlagBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildFlag(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map, [Combine] FlagNode flag, [JoinByTeam] TeamNode teamNode)
        {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            TeamColor teamColor = teamNode.colorInBattle.TeamColor;
            GameObject original = (teamColor != TeamColor.RED) ? assetProxyBehaviour.blueFlag : assetProxyBehaviour.redFlag;
            GameObject obj3 = (teamColor != TeamColor.RED) ? assetProxyBehaviour.blueFlagBeam : assetProxyBehaviour.redFlagBeam;
            FlagInstanceComponent component = new FlagInstanceComponent();
            GameObject obj4 = Object.Instantiate<GameObject>(original, flag.flagPosition.Position, Quaternion.identity);
            component.FlagInstance = obj4;
            component.FlagBeam = Object.Instantiate<GameObject>(obj3, obj4.transform, false);
            flag.Entity.AddComponent(component);
            obj4.AddComponent<FlagPhysicsBehaviour>().TriggerEntity = flag.Entity;
            flag.Entity.AddComponent(new FlagColliderComponent(obj4.GetComponent<BoxCollider>()));
        }

        [OnEventFire]
        public void BuildPedestal(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map, [Combine] FlagPedestalNode flagPedestal, [JoinByTeam] TeamNode teamNode)
        {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            FlagPedestalInstanceComponent component = new FlagPedestalInstanceComponent {
                FlagPedestalInstance = Object.Instantiate<GameObject>((teamNode.colorInBattle.TeamColor != TeamColor.RED) ? assetProxyBehaviour.bluePedestal : assetProxyBehaviour.redPedestal, flagPedestal.flagPedestal.Position, Quaternion.identity)
            };
            flagPedestal.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagInstanceComponent> flag)
        {
            Object.Destroy(flag.component.FlagInstance);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagPedestalInstanceComponent> pedestal)
        {
            Object.Destroy(pedestal.component.FlagPedestalInstance);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, TeamNode team, [JoinByTeam] SingleNode<FlagInstanceComponent> flag)
        {
            Object.Destroy(flag.component.FlagInstance);
        }

        [OnEventFire]
        public void DestroyPedestal(NodeRemoveEvent e, TeamNode team, [JoinByTeam] SingleNode<FlagPedestalInstanceComponent> pedestal)
        {
            Object.Destroy(pedestal.component.FlagPedestalInstance);
        }

        private static CTFAssetProxyBehaviour GetAssetProxyBehaviour(BattleNode ctf) => 
            ((GameObject) ctf.resourceData.Data).GetComponent<CTFAssetProxyBehaviour>();

        public class BattleNode : Node
        {
            public CTFComponent ctf;
            public SelfComponent self;
            public ResourceDataComponent resourceData;
            public BattleGroupComponent battleGroup;
        }

        public class FlagNode : Node
        {
            public FlagPositionComponent flagPosition;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
        }

        public class FlagPedestalNode : Node
        {
            public FlagPedestalComponent flagPedestal;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
        }

        public class TeamNode : Node
        {
            public ColorInBattleComponent colorInBattle;
            public TeamColorComponent teamColor;
        }
    }
}

