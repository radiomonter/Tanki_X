namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class BattleSelectLoadSystem : ECSSystem
    {
        [OnEventComplete]
        public void AttachScreenToTargeBattle(ShowScreenEvent e, SingleNode<BattleGroupComponent> node, [JoinAll] UngroupedBattleSelectLoadScreenNode screen)
        {
            screen.Entity.AddComponent(new BattleGroupComponent(node.component.Key));
        }

        private int GetEffectiveLevel(Optional<MountedWeaponNode> weapon, Optional<MountedHullNode> hull) => 
            (!weapon.IsPresent() || !hull.IsPresent()) ? BattleSelectSystem.TRAIN_BATTLE_MAXIMAL_RANK : Math.Max(weapon.Get().upgradeLevelItem.Level, hull.Get().upgradeLevelItem.Level);

        [OnEventFire]
        public void ShowBattleSelect(NodeAddedEvent e, BattleSelectLoadScreenNode screen, UserReadyForLobbyNode user, [JoinAll] SelfUserNode selfUser, [JoinByUser] Optional<MountedWeaponNode> weapon, [JoinAll] SelfUserNode selfUser2, [JoinByUser] Optional<MountedHullNode> hull)
        {
            if (this.GetEffectiveLevel(weapon, hull) < BattleSelectSystem.TRAIN_BATTLE_MAXIMAL_RANK)
            {
                base.ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(user);
            }
            else
            {
                base.ScheduleEvent(new ShowBattleEvent(screen.battleGroup.Key), EngineService.EntityStub);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class BattleSelectLoadScreenNode : Node
        {
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
            public BattleGroupComponent battleGroup;
        }

        public class MountedHullNode : Node
        {
            public TankItemComponent tankItem;
            public MountedItemComponent mountedItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class MountedWeaponNode : Node
        {
            public WeaponItemComponent weaponItem;
            public MountedItemComponent mountedItem;
            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        [Not(typeof(BattleGroupComponent))]
        public class UngroupedBattleSelectLoadScreenNode : Node
        {
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
        }

        public class UserReadyForLobbyNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}

