namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientGarage.API;

    public class RoundUserEquipmentSystem : ECSSystem
    {
        [OnEventFire]
        public void SaveEquipment(NodeAddedEvent e, [Combine] RoundUserNode user, [Context, JoinByUser] HullNode hull, [Context, JoinByUser] TurretNode turret)
        {
            if (user.Entity.HasComponent<RoundUserEquipmentComponent>())
            {
                RoundUserEquipmentComponent component = user.Entity.GetComponent<RoundUserEquipmentComponent>();
                component.HullId = hull.marketItemGroup.Key;
                component.WeaponId = turret.marketItemGroup.Key;
            }
            else
            {
                RoundUserEquipmentComponent component = new RoundUserEquipmentComponent {
                    HullId = hull.marketItemGroup.Key,
                    WeaponId = turret.marketItemGroup.Key
                };
                user.Entity.AddComponent(component);
            }
        }

        public class HullNode : Node
        {
            public UserGroupComponent userGroup;
            public TankComponent tank;
            public MarketItemGroupComponent marketItemGroup;
            public TankPartComponent tankPart;
        }

        public class RoundUserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
        }

        public class TurretNode : Node
        {
            public UserGroupComponent userGroup;
            public WeaponComponent weapon;
            public MarketItemGroupComponent marketItemGroup;
            public TankPartComponent tankPart;
        }
    }
}

