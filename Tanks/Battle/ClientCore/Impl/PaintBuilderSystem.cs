namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class PaintBuilderSystem : ECSSystem
    {
        [OnEventComplete]
        public void InstantiateAndPreparePaint(NodeAddedEvent e, TankPaintBattleItemNode paintBattleItem, [Context, JoinByTank] TankNode tank)
        {
            Transform parent = tank.hullInstance.HullInstance.transform;
            GameObject paintInstance = Object.Instantiate(paintBattleItem.resourceData.Data, parent) as GameObject;
            tank.Entity.AddComponent(new TankPartPaintInstanceComponent(paintInstance));
        }

        [OnEventComplete]
        public void InstantiateAndPreparePaint(NodeAddedEvent e, WeaponPaintBattleItemNode paintBattleItem, [Context, JoinByTank] WeaponNode weapon)
        {
            Transform parent = weapon.weaponInstance.WeaponInstance.transform;
            GameObject paintInstance = Object.Instantiate(paintBattleItem.resourceData.Data, parent) as GameObject;
            weapon.Entity.AddComponent(new TankPartPaintInstanceComponent(paintInstance));
        }

        public class TankNode : Node
        {
            public BattleGroupComponent battleGroup;
            public TankComponent tank;
            public HullInstanceComponent hullInstance;
            public TankGroupComponent tankGroup;
        }

        public class TankPaintBattleItemNode : Node
        {
            public TankPaintBattleItemComponent tankPaintBattleItem;
            public ResourceDataComponent resourceData;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public BattleGroupComponent battleGroup;
            public WeaponComponent weapon;
            public WeaponInstanceComponent weaponInstance;
            public TankGroupComponent tankGroup;
        }

        public class WeaponPaintBattleItemNode : Node
        {
            public WeaponPaintBattleItemComponent weaponPaintBattleItem;
            public ResourceDataComponent resourceData;
            public TankGroupComponent tankGroup;
        }
    }
}

