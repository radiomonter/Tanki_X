namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class UserEquipmentSystem : ECSSystem
    {
        [OnEventFire]
        public void sendUserEquipmentOnEnterLobby(NodeAddedEvent e, UserInMatchMakingLobby user, [Context, JoinByUser] Weapon weapon, [Context, JoinByUser] Hull hull, [JoinByUser] UserInMatchMakingLobby user2Lobby, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby)
        {
            SetEquipmentEvent eventInstance = new SetEquipmentEvent {
                WeaponId = weapon.marketItemGroup.Key,
                HullId = hull.marketItemGroup.Key
            };
            base.ScheduleEvent(eventInstance, lobby);
            user.Entity.RemoveComponentIfPresent<InitUserEquipmentComponent>();
            user.Entity.AddComponent<InitUserEquipmentComponent>();
        }

        [OnEventFire]
        public void sendUserEquipmentOnEnterLobby(NodeAddedEvent e, UserInMatchMakingLobbyPrototype user, [Context, JoinByUser] Weapon weapon, [Context, JoinByUser] Hull hull, [JoinByUser] UserInMatchMakingLobby user2Lobby, [JoinByBattleLobby] SingleNode<BattleLobbyComponent> lobby)
        {
            SetEquipmentEvent eventInstance = new SetEquipmentEvent();
            if (user.userUseItemsPrototype.Preset.HasComponent<PresetEquipmentComponent>())
            {
                PresetEquipmentComponent component = user.userUseItemsPrototype.Preset.GetComponent<PresetEquipmentComponent>();
                eventInstance.WeaponId = Flow.Current.EntityRegistry.GetEntity(component.WeaponId).GetComponent<MarketItemGroupComponent>().Key;
                eventInstance.HullId = Flow.Current.EntityRegistry.GetEntity(component.HullId).GetComponent<MarketItemGroupComponent>().Key;
                base.ScheduleEvent(eventInstance, lobby);
            }
        }

        public class Hull : Node
        {
            public TankItemComponent tankItem;
            public MountedItemComponent mountedItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class InitUserEquipmentComponent : Component
        {
        }

        public class UserInMatchMakingLobby : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class UserInMatchMakingLobbyPrototype : UserEquipmentSystem.UserInMatchMakingLobby
        {
            public UserEquipmentSystem.InitUserEquipmentComponent initUserEquipment;
            public UserUseItemsPrototypeComponent userUseItemsPrototype;
        }

        public class Weapon : Node
        {
            public WeaponItemComponent weaponItem;
            public MountedItemComponent mountedItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

