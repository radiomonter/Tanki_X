namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class CreateCustomBattleSystem : ECSSystem
    {
        private static Entity screen;
        [CompilerGenerated]
        private static OnDropDownListItemSelected <>f__mg$cache0;
        [CompilerGenerated]
        private static OnDropDownListItemSelected <>f__mg$cache1;
        [CompilerGenerated]
        private static OnDropDownListItemSelected <>f__mg$cache2;
        [CompilerGenerated]
        private static Func<BattleMode, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache3;

        private ClientBattleParams CollectBattleParams(ScreenNode screen, ICollection<MapNode> maps)
        {
            <CollectBattleParams>c__AnonStorey2 storey = new <CollectBattleParams>c__AnonStorey2 {
                component = screen.createBattleScreen
            };
            ClientBattleParams battleParams = new ClientBattleParams {
                BattleMode = GetBattleMode(storey.component),
                MapId = maps.First<MapNode>(new Func<MapNode, bool>(storey.<>m__0)).Entity.Id,
                MaxPlayers = GetPlayerLimit(storey.component),
                TimeLimit = GetTimeLimit(storey.component),
                ScoreLimit = 0,
                FriendlyFire = storey.component.friendlyFireDropdown.SelectionIndex == 1,
                Gravity = GetGravityType(storey.component),
                KillZoneEnabled = storey.component.killZoneDropdown.SelectionIndex == 1,
                DisabledModules = storey.component.enabledModulesDropdown.SelectionIndex == 0
            };
            SaveParams(storey.component, battleParams);
            return battleParams;
        }

        [OnEventFire]
        public void DeinitSelfLobby(NodeRemoveEvent e, LobbyWithUserGroupNode lobby)
        {
            if (lobby.Entity.HasComponent<SelfComponent>())
            {
                lobby.Entity.RemoveComponent<SelfComponent>();
            }
        }

        [OnEventFire]
        public void EditBattleParams(ButtonClickEvent e, EditBattleParamsButtonNode button)
        {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.CreateBattle, false);
        }

        private void EnableButton(Button button, bool enabled)
        {
            button.interactable = enabled;
            button.gameObject.SetActive(enabled);
        }

        private static BattleMode GetBattleMode(CreateBattleScreenComponent component) => 
            ((BattleMode[]) Enum.GetValues(typeof(BattleMode)))[component.battleModeDropdown.SelectionIndex];

        private static GravityType GetGravityType(CreateBattleScreenComponent component)
        {
            int selectionIndex = component.gravityDropdown.SelectionIndex;
            return (GravityType) Enum.GetValues(typeof(GravityType)).GetValue(selectionIndex);
        }

        private static int GetPlayerLimit(CreateBattleScreenComponent component)
        {
            int result = 20;
            string selected = (string) component.maxPlayersDropdown.Selected;
            if (selected != null)
            {
                int.TryParse(selected, out result);
                if (GetBattleMode(component) != BattleMode.DM)
                {
                    result *= 2;
                }
            }
            return result;
        }

        private static int GetScoreLimit(CreateBattleScreenComponent component)
        {
            int result = 0;
            string selected = (string) component.scoreLimitDropdown.Selected;
            if (selected != null)
            {
                int.TryParse(selected, out result);
            }
            return result;
        }

        private static int GetTimeLimit(CreateBattleScreenComponent component)
        {
            int result = 0;
            char[] separator = new char[] { ' ' };
            int.TryParse(((string) component.timeLimitDropdown.Selected).Split(separator)[0], out result);
            return result;
        }

        private static void GroupMapWithScreen(string mapName)
        {
            <GroupMapWithScreen>c__AnonStorey1 storey = new <GroupMapWithScreen>c__AnonStorey1 {
                mapName = mapName
            };
            if (screen.HasComponent<MapGroupComponent>())
            {
                screen.RemoveComponent<MapGroupComponent>();
            }
            NodeClassInstanceDescription orCreateNodeClassDescription = EngineImpl.NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(MapNode), null);
            Entity entity = Flow.Current.NodeCollector.GetEntities(orCreateNodeClassDescription.NodeDescription).First<Entity>(new Func<Entity, bool>(storey.<>m__0));
            screen.AddComponent(new MapGroupComponent(entity.Id));
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] ICollection<MapNode> mapNodes, [JoinAll] Optional<LobbyNode> lobby, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            // Unresolved stack state at '00000598'
        }

        [OnEventFire]
        public void JoinTeam(ButtonClickEvent e, SingleNode<SwitchTeamButtonComponent> button, [JoinAll] LobbyNode lobby)
        {
            base.ScheduleEvent<SwitchTeamEvent>(lobby);
        }

        private static ClientBattleParams LoadParams() => 
            new ClientBattleParams { 
                BattleMode = (BattleMode) Enum.GetValues(typeof(BattleMode)).GetValue(PlayerPrefs.GetInt("BattleParams.BattleMode")),
                MapId = PlayerPrefs.GetInt("BattleParams.MapId"),
                MaxPlayers = PlayerPrefs.GetInt("BattleParams.MaxPlayers"),
                TimeLimit = PlayerPrefs.GetInt("BattleParams.TimeLimit"),
                ScoreLimit = PlayerPrefs.GetInt("BattleParams.ScoreLimit"),
                FriendlyFire = PlayerPrefs.GetInt("BattleParams.FriendlyFire") != 0,
                Gravity = (GravityType) Enum.GetValues(typeof(GravityType)).GetValue(PlayerPrefs.GetInt("BattleParams.Gravity")),
                KillZoneEnabled = PlayerPrefs.GetInt("BattleParams.KillZoneDisabled") == 0,
                DisabledModules = PlayerPrefs.GetInt("BattleParams.EnabledModules") == 0
            };

        private static void OnBattleModeSelected(ListItem item)
        {
            CreateBattleScreenComponent component = screen.GetComponent<CreateBattleScreenComponent>();
            UpdatePlayerLimit(component, -1);
            UpdateScoreLimit(component, 0);
            UpdateFriendlyFire(component);
            UpdateMaps(component);
        }

        private static void OnMapSelected(ListItem item)
        {
            GroupMapWithScreen((string) item.Data);
        }

        private static void OnTimeLimitSelected(ListItem item)
        {
            UpdateScoreLimit(screen.GetComponent<CreateBattleScreenComponent>(), 0);
        }

        [OnEventFire]
        public void OnUpdateResponse(NodeAddedEvent e, SingleNode<ClientBattleParamsComponent> lobby, [JoinAll] ScreenNode screen)
        {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby, true);
        }

        [OnEventFire]
        public void RegisterMapEnabledInCustomComponent(NodeAddedEvent e, SingleNode<MapEnabledInCustomGameComponent> map)
        {
        }

        [OnEventFire]
        public void RequestMapPreview(NodeAddedEvent e, ScreenWithMapGroupNode screen, [JoinByMap] MapNode map)
        {
            if (map.Entity.HasComponent<MapPreviewDataComponent>())
            {
                SetPreviewImage(screen, map);
            }
            else
            {
                AssetRequestEvent eventInstance = new AssetRequestEvent();
                eventInstance.Init<MapPreviewDataComponent>(map.mapPreview.AssetGuid);
                base.ScheduleEvent(eventInstance, map);
            }
            screen.createBattleScreen.mapName.text = map.descriptionItem.Name;
        }

        private static void SaveParams(CreateBattleScreenComponent component, ClientBattleParams battleParams)
        {
            PlayerPrefs.SetInt("BattleParams.BattleMode", component.battleModeDropdown.SelectionIndex);
            PlayerPrefs.SetInt("BattleParams.MapId", (int) battleParams.MapId);
            PlayerPrefs.SetInt("BattleParams.MaxPlayers", battleParams.MaxPlayers);
            PlayerPrefs.SetInt("BattleParams.TimeLimit", battleParams.TimeLimit);
            PlayerPrefs.SetInt("BattleParams.ScoreLimit", battleParams.ScoreLimit);
            PlayerPrefs.SetInt("BattleParams.FriendlyFire", !battleParams.FriendlyFire ? 0 : 1);
            PlayerPrefs.SetInt("BattleParams.Gravity", component.gravityDropdown.SelectionIndex);
            PlayerPrefs.SetInt("BattleParams.KillZoneDisabled", !battleParams.KillZoneEnabled ? 1 : 0);
            PlayerPrefs.SetInt("BattleParams.EnabledModules", !battleParams.DisabledModules ? 1 : 0);
            PlayerPrefs.Save();
        }

        [OnEventFire]
        public void SendCreateBattle(ButtonClickEvent e, CreateBattleButtonNode button, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] ScreenNode screen, [JoinAll] ICollection<MapNode> maps, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            if (selfUser.Entity.HasComponent<SquadGroupComponent>() && !selfUser.Entity.HasComponent<SquadLeaderComponent>())
            {
                CantStartGameInSquadDialogComponent component = dialogs.component.Get<CantStartGameInSquadDialogComponent>();
                component.CantSearch = false;
                component.Show(null);
            }
            else
            {
                button.buttonMapping.Button.interactable = false;
                CreateCustomBattleLobbyEvent eventInstance = new CreateCustomBattleLobbyEvent {
                    Params = this.CollectBattleParams(screen, maps)
                };
                base.NewEvent(eventInstance).Attach(session).ScheduleDelayed(0.3f);
            }
        }

        [OnEventFire]
        public void SendUpdateBattleParams(ButtonClickEvent e, UpdateBattleParamsButtonNode button, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] LobbyNode lobby, [JoinAll] ScreenNode screen, [JoinAll] ICollection<MapNode> maps)
        {
            button.buttonMapping.Button.interactable = false;
            UpdateBattleParamsEvent eventInstance = new UpdateBattleParamsEvent {
                Params = this.CollectBattleParams(screen, maps)
            };
            base.ScheduleEvent(eventInstance, lobby);
        }

        private static void SetPreviewImage(ScreenNode screen, MapNode map)
        {
            screen.createBattleScreen.mapPreviewRawImage.texture = (Texture2D) map.Entity.GetComponent<MapPreviewDataComponent>().Data;
        }

        [OnEventFire]
        public void ShowMapPreviewOnLoad(NodeAddedEvent e, MapWithPreviewDataNode map, [JoinByMap] ScreenNode screen)
        {
            SetPreviewImage(screen, map);
        }

        private static void UpdateFriendlyFire(CreateBattleScreenComponent component)
        {
            DefaultDropDownList friendlyFireDropdown = component.friendlyFireDropdown;
            Button button = friendlyFireDropdown.GetComponent<Button>();
            button.interactable = GetBattleMode(component) != BattleMode.DM;
            if (!button.interactable)
            {
                friendlyFireDropdown.SelectionIndex = 0;
            }
        }

        private static void UpdateMaps(CreateBattleScreenComponent component)
        {
            BattleMode battleMode = GetBattleMode(component);
            List<string> source = component.ModesToMapsDict[battleMode];
            string item = source.First<string>();
            component.mapDropdown.UpdateList(source, source.IndexOf(item));
            GroupMapWithScreen(item);
        }

        private static void UpdatePlayerLimit(CreateBattleScreenComponent component, int savedPlayerLimit = -1)
        {
            string[] textArray3;
            BattleMode battleMode = GetBattleMode(component);
            if (battleMode == BattleMode.DM)
            {
                string[] textArray1 = new string[10];
                textArray1[0] = "2";
                textArray1[1] = "4";
                textArray1[2] = "6";
                textArray1[3] = "8";
                textArray1[4] = "10";
                textArray1[5] = "12";
                textArray1[6] = "14";
                textArray1[7] = "16";
                textArray1[8] = "18";
                textArray1[9] = "20";
                textArray3 = textArray1;
            }
            else
            {
                string[] textArray2 = new string[10];
                textArray2[0] = "1";
                textArray2[1] = "2";
                textArray2[2] = "3";
                textArray2[3] = "4";
                textArray2[4] = "5";
                textArray2[5] = "6";
                textArray2[6] = "7";
                textArray2[7] = "8";
                textArray2[8] = "9";
                textArray2[9] = "10";
                textArray3 = textArray2;
            }
            string[] array = textArray3;
            component.maxPlayerText.text = (battleMode != BattleMode.DM) ? ((string) component.maxTeamPlayerText) : ((string) component.maxDmPlayerText);
            int selectionIndex = component.maxPlayersDropdown.SelectionIndex;
            if (selectionIndex < 0)
            {
                selectionIndex = Array.IndexOf<string>(array, savedPlayerLimit.ToString());
                if (selectionIndex < 0)
                {
                    selectionIndex = array.Length - 1;
                }
            }
            component.maxPlayersDropdown.UpdateList(array.ToList<string>(), selectionIndex);
        }

        private static void UpdateScoreLimit(CreateBattleScreenComponent component, int savedScoreLimit = 0)
        {
            BattleMode battleMode = GetBattleMode(component);
            string noLimitText = (string) component.noLimitText;
            string[] array = new string[0];
            if (battleMode == BattleMode.DM)
            {
                array = new string[] { noLimitText, "10", "20", "30", "50" };
            }
            else if (battleMode == BattleMode.TDM)
            {
                array = new string[] { noLimitText, "10", "20", "30", "50" };
            }
            else if (battleMode == BattleMode.CTF)
            {
                array = new string[] { noLimitText, "1", "3", "5", "10" };
            }
            int selectionIndex = component.scoreLimitDropdown.SelectionIndex;
            if (selectionIndex < 0)
            {
                selectionIndex = Array.IndexOf<string>(array, savedScoreLimit.ToString());
                if (selectionIndex < 0)
                {
                    selectionIndex = 0;
                }
            }
            component.scoreLimitDropdown.UpdateList(array.ToList<string>(), selectionIndex);
        }

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }

        [CompilerGenerated]
        private sealed class <CollectBattleParams>c__AnonStorey2
        {
            internal CreateBattleScreenComponent component;

            internal bool <>m__0(CreateCustomBattleSystem.MapNode map) => 
                map.descriptionItem.Name.Equals(this.component.mapDropdown.Selected);
        }

        [CompilerGenerated]
        private sealed class <GroupMapWithScreen>c__AnonStorey1
        {
            internal string mapName;

            internal bool <>m__0(Entity m) => 
                m.GetComponent<DescriptionItemComponent>().Name.Equals(this.mapName);
        }

        [CompilerGenerated]
        private sealed class <InitScreen>c__AnonStorey0
        {
            internal Dictionary<BattleMode, string> modeNames;
            internal ClientBattleParams p;
            internal string minutesText;
            internal Dictionary<GravityType, string> gravityNames;

            internal string <>m__0(BattleMode en) => 
                this.modeNames[en];

            internal bool <>m__1(CreateCustomBattleSystem.MapNode n) => 
                n.Entity.Id == this.p.MapId;

            internal string <>m__2(int n) => 
                n + this.minutesText;

            internal string <>m__3(GravityType g) => 
                this.gravityNames[g];
        }

        public class CreateBattleButtonNode : Node
        {
            public CreateBattleButtonComponent createBattleButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class EditBattleParamsButtonNode : Node
        {
            public EditCustomBattleParamsButtonComponent editCustomBattleParamsButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class LobbyNode : Node
        {
            public CustomBattleLobbyComponent customBattleLobby;
            public UserGroupComponent userGroup;
            public ClientBattleParamsComponent clientBattleParams;
        }

        public class LobbyWithUserGroupNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public UserGroupComponent userGroup;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public DescriptionItemComponent descriptionItem;
            public MapPreviewComponent mapPreview;
            public MapModeRestrictionComponent mapModeRestriction;
        }

        public class MapWithPreviewDataNode : CreateCustomBattleSystem.MapNode
        {
            public MapPreviewDataComponent mapPreviewData;
        }

        public class ScreenNode : Node
        {
            public CreateBattleScreenComponent createBattleScreen;
        }

        public class ScreenWithMapGroupNode : CreateCustomBattleSystem.ScreenNode
        {
            public MapGroupComponent mapGroup;
        }

        public class UpdateBattleParamsButtonNode : Node
        {
            public UpdateBattleParamsComponent updateBattleParams;
            public ButtonMappingComponent buttonMapping;
        }
    }
}

