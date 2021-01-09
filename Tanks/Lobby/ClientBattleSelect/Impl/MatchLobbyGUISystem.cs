namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class MatchLobbyGUISystem : ECSSystem
    {
        private void CheckForDeserterDesc(SelfUserNode selfUser)
        {
            int needGoodBattles = selfUser.battleLeaveCounter.NeedGoodBattles;
            if (needGoodBattles > 0)
            {
                MainScreenComponent.Instance.ShowDeserterDesc(needGoodBattles, true);
            }
            else
            {
                MainScreenComponent.Instance.HideDeserterDesc();
            }
        }

        [OnEventFire]
        public void CheckForShowSpectatorModeButton(CheckForSpectatorButtonShowEvent e, Node any, [JoinAll] Optional<SelfBattleLobbyUser> selfLobbyUserNode, [JoinAll] Optional<SelfSquadUser> selfSquadUser)
        {
            e.CanGoToSpectatorMode = !selfLobbyUserNode.IsPresent() && !selfSquadUser.IsPresent();
        }

        [OnEventFire]
        public void DeinitUI(NodeRemoveEvent e, LobbyNode lobby, SingleNode<MatchLobbyGUIComponent> ui)
        {
            if (lobby.Entity.HasComponent<BattleLobbyGroupComponent>() && ui.Entity.HasComponent<BattleLobbyGroupComponent>())
            {
                lobby.battleLobbyGroup.Detach(ui.Entity);
            }
        }

        private string GetPresetName(PresetNode preset)
        {
            GetPresetNameEvent eventInstance = new GetPresetNameEvent();
            base.ScheduleEvent(eventInstance, preset);
            return eventInstance.Name;
        }

        [OnEventFire]
        public void InitHullForBattleSelectScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, HullNode hull, [Context, JoinByMarketItem] MarketItemNode marketItem)
        {
            ui.component.Hull = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
            ui.component.SetHullLabels();
        }

        [OnEventFire]
        public void InitTurretForBattleSelectScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, TurretNode turret, [Context, JoinByMarketItem] MarketItemNode marketItem)
        {
            ui.component.Turret = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
            ui.component.SetTurretLabels();
        }

        [OnEventFire]
        public void InitUI(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, LobbyNode lobby, [JoinByMap] MapNode map)
        {
            GameModesDescriptionData data = ConfiguratorService.GetConfig("localization/battle_mode").ConvertTo<GameModesDescriptionData>();
            ui.component.SetTeamBattleMode(lobby.battleMode.BattleMode != BattleMode.DM, lobby.userLimit.TeamLimit, lobby.userLimit.UserLimit);
            ui.component.ModeName = data.battleModeLocalization[lobby.battleMode.BattleMode];
            ui.component.MapName = map.descriptionItem.Name;
            ui.component.ShowSearchingText = !lobby.Entity.HasComponent<CustomBattleLobbyComponent>();
            if (map.Entity.HasComponent<MapPreviewDataComponent>())
            {
                ui.component.SetMapPreview((Texture2D) map.Entity.GetComponent<MapPreviewDataComponent>().Data);
            }
            else
            {
                AssetRequestEvent eventInstance = new AssetRequestEvent();
                eventInstance.Init<MapPreviewDataComponent>(map.mapPreview.AssetGuid);
                base.ScheduleEvent(eventInstance, map);
            }
            if (ui.Entity.HasComponent<BattleLobbyGroupComponent>())
            {
                ui.Entity.GetComponent<BattleLobbyGroupComponent>().Detach(ui.Entity);
            }
            lobby.battleLobbyGroup.Attach(ui.Entity);
            ui.component.paramGravity.text = ConfiguratorService.GetConfig("localization/gravity_type").ConvertTo<GravityTypeNames>().Names[lobby.gravity.GravityType];
        }

        [OnEventFire]
        public void ItemMounted(NodeAddedEvent e, SingleNode<MountedItemComponent> node, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui)
        {
            ui.component.InitHullDropDowns();
            ui.component.InitTurretDropDowns();
        }

        [OnEventFire]
        public void LobbyMasterAdded(NodeAddedEvent e, CustomLobbyNode lobbyWithMaster, [JoinByUser] SingleNode<LobbyUserListItemComponent> userUI)
        {
            userUI.component.Master = true;
        }

        [OnEventFire]
        public void LobbyMasterRemoved(NodeRemoveEvent e, CustomLobbyNode lobbyWithMaster, [JoinByUser] SingleNode<LobbyUserListItemComponent> userUI)
        {
            userUI.component.Master = false;
        }

        [OnEventFire]
        public void LobbyRemove(NodeRemoveEvent e, SingleNode<MatchLobbyGUIComponent> gameModeSelectScreen)
        {
            MainScreenComponent.Instance.HideDeserterDesc();
        }

        [OnEventFire]
        public void MatchLobbyGUIAdded(NodeAddedEvent e, LobbyUINode matchLobbyGUI, [Combine, JoinByBattleLobby, Context] UserNode userNode, [JoinByBattleLobby] Optional<CustomLobbyNode> customLobby, [JoinAll] SelfBattleLobbyUser selfBattleLobbyUser)
        {
            bool selfUser = userNode.userGroup.Key == selfBattleLobbyUser.userGroup.Key;
            matchLobbyGUI.matchLobbyGUI.AddUser(userNode.Entity, selfUser, ((userNode.Entity.HasComponent<SquadGroupComponent>() && selfBattleLobbyUser.Entity.HasComponent<SquadGroupComponent>()) && (userNode.Entity.GetComponent<SquadGroupComponent>().Key == selfBattleLobbyUser.Entity.GetComponent<SquadGroupComponent>().Key)) || customLobby.IsPresent());
        }

        [OnEventFire]
        public void MatchLobbyGUIRemoved(NodeRemoveEvent e, LobbyUINode ui, [Combine, JoinByBattleLobby, Context] UserNode userNode)
        {
            if (userNode.Entity.HasComponent<LobbyUserListItemComponent>())
            {
                userNode.Entity.RemoveComponent<LobbyUserListItemComponent>();
            }
            ui.matchLobbyGUI.RemoveUser(userNode.Entity);
        }

        [OnEventFire]
        public void MatchLobbyGUIShowed(MatchLobbyShowEvent e, SingleNode<MatchLobbyGUIComponent> ui, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] ICollection<PresetNode> presetsList)
        {
            List<PresetItem> items = new List<PresetItem>();
            foreach (PresetNode node in presetsList)
            {
                if (node.presetEquipment.HullId == 0L)
                {
                }
                if (node.presetEquipment.WeaponId == 0L)
                {
                }
                if ((node.presetEquipment.HullId != 0L) && (node.presetEquipment.WeaponId != 0L))
                {
                    Entity entityById = base.GetEntityById(node.presetEquipment.HullId);
                    Entity entity2 = base.GetEntityById(node.presetEquipment.WeaponId);
                    items.Add(new PresetItem(this.GetPresetName(node), 1, entityById.GetComponent<DescriptionItemComponent>().Name, entity2.GetComponent<DescriptionItemComponent>().Name, entityById.GetComponent<MarketItemGroupComponent>().Key, entity2.GetComponent<MarketItemGroupComponent>().Key, node.Entity));
                }
            }
            items.Sort(new PresetItemComparer());
            ui.component.InitPresetsDropDown(items);
        }

        [OnEventFire]
        public void OnLobbyScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> gameModeSelectScreen, [JoinAll] SelfUserNode selfUser, [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customBattleLobby)
        {
            if (customBattleLobby.IsPresent())
            {
                MainScreenComponent.Instance.HideDeserterDesc();
            }
            else
            {
                this.CheckForDeserterDesc(selfUser);
            }
        }

        [OnEventFire]
        public void OnMainScreen(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, [JoinAll, Context] SelfUserNode selfUser)
        {
            int needGoodBattles = selfUser.battleLeaveCounter.NeedGoodBattles;
            if (needGoodBattles > 0)
            {
                MainScreenComponent.Instance.ShowDesertIcon(needGoodBattles);
            }
            else
            {
                MainScreenComponent.Instance.HideDeserterIcon();
            }
        }

        [OnEventFire]
        public void SetPreview(NodeAddedEvent e, MapWithPreviewDataNode map, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui)
        {
            ui.component.SetMapPreview((Texture2D) map.mapPreviewData.Data);
        }

        [OnEventFire]
        public void SetSelfUserTeamColor(NodeAddedEvent e, SelfBattleLobbyUser selfUserNode, [JoinByBattleLobby] LobbyNode lobby, [JoinAll, Context] LobbyUINode ui)
        {
            ui.matchLobbyGUI.UserTeamColor = !lobby.Entity.HasComponent<CustomBattleLobbyComponent>() ? TeamColor.NONE : selfUserNode.teamColor.TeamColor;
        }

        private void UpdateEquipment(LobbyUserListItemComponent uiItem, UserEquipmentComponent userEquipment)
        {
            Entity entityById = base.GetEntityById(userEquipment.WeaponId);
            Entity marketEntity = base.GetEntityById(userEquipment.HullId);
            TankPartItem item = GarageItemsRegistry.GetItem<TankPartItem>(marketEntity);
            TankPartItem item2 = GarageItemsRegistry.GetItem<TankPartItem>(entityById);
            uiItem.UpdateEquipment(item.Name, userEquipment.HullId, item2.Name, userEquipment.WeaponId);
        }

        [OnEventFire]
        public void UserEquipment(NodeAddedEvent e, UserEquipmentNode userEquipment, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui)
        {
            this.UpdateEquipment(userEquipment.lobbyUserListItem, userEquipment.userEquipment);
        }

        [OnEventFire]
        public void UserItemAdded(NodeAddedEvent e, SingleNode<LobbyUserListItemComponent> userUI, [JoinByUser] Optional<CustomLobbyNode> lobby)
        {
            bool flag = lobby.IsPresent();
            userUI.component.Master = flag;
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }

        public class CustomLobbyNode : Node
        {
            public CustomBattleLobbyComponent customBattleLobby;
            public UserGroupComponent userGroup;
        }

        public class HullNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
            public TankItemComponent tankItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class LobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public BattleLobbyGroupComponent battleLobbyGroup;
            public MapGroupComponent mapGroup;
            public BattleModeComponent battleMode;
            public UserLimitComponent userLimit;
            public GravityComponent gravity;
        }

        public class LobbyUINode : Node
        {
            public MatchLobbyGUIComponent matchLobbyGUI;
            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public MapGroupComponent mapGroup;
            public DescriptionItemComponent descriptionItem;
            public MapPreviewComponent mapPreview;
        }

        public class MapWithPreviewDataNode : MatchLobbyGUISystem.MapNode
        {
            public MapPreviewDataComponent mapPreviewData;
        }

        public class MarketItemNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
        }

        private class PresetItemComparer : IComparer<PresetItem>
        {
            public int Compare(PresetItem p1, PresetItem p2) => 
                PresetSystem.comparer.Compare(p1.presetEntity, p2.presetEntity);
        }

        public class PresetNode : Node
        {
            public PresetItemComponent presetItem;
            public PresetNameComponent presetName;
            public UserItemComponent userItem;
            public PresetEquipmentComponent presetEquipment;
        }

        public class SelfBattleLobbyUser : MatchLobbyGUISystem.UserNode
        {
            public SelfUserComponent selfUser;
        }

        public class SelfSquadUser : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
            public BattleLeaveCounterComponent battleLeaveCounter;
        }

        public class TurretNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
            public WeaponItemComponent weaponItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class UserEquipmentNode : MatchLobbyGUISystem.UserNode
        {
            public UserEquipmentComponent userEquipment;
            public LobbyUserListItemComponent lobbyUserListItem;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public BattleLobbyGroupComponent battleLobbyGroup;
            public UserUidComponent userUid;
            public UserGroupComponent userGroup;
            public TeamColorComponent teamColor;
            public BattleLeaveCounterComponent battleLeaveCounter;
        }
    }
}

