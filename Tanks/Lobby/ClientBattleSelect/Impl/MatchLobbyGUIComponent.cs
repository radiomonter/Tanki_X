namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MatchLobbyGUIComponent : EventMappingComponent
    {
        private bool teamBattleMode;
        [SerializeField]
        private GameObject teamList1Title;
        [SerializeField]
        private TextMeshProUGUI gameModeTitle;
        [SerializeField]
        private Image mapIcon;
        [SerializeField]
        private TextMeshProUGUI mapTitle;
        [SerializeField]
        private PresetsDropDownList presetsDropDownList;
        [SerializeField]
        private VisualItemsDropDownList hullSkinsDropDownList;
        [SerializeField]
        private VisualItemsDropDownList hullPaintDropDownList;
        [SerializeField]
        private VisualItemsDropDownList turretSkinsDropDownList;
        [SerializeField]
        private VisualItemsDropDownList turretPaintDropDownList;
        [SerializeField]
        private RectTransform blueTeamListWithHeader;
        [SerializeField]
        private RectTransform redTeamListWithHeader;
        [SerializeField]
        private TeamListGUIComponent blueTeamList;
        [SerializeField]
        private TeamListGUIComponent redTeamList;
        [SerializeField]
        private TextMeshProUGUI hullName;
        [SerializeField]
        private TextMeshProUGUI turretName;
        [SerializeField]
        private TextMeshProUGUI hullFeature;
        [SerializeField]
        private TextMeshProUGUI turretFeature;
        [SerializeField]
        private GameObject customGameElements;
        [SerializeField]
        private GameObject openBattleButton;
        [SerializeField]
        private GameObject inviteFriendsButton;
        [SerializeField]
        private GameObject editParamsButton;
        public TextMeshProUGUI paramTimeLimit;
        public TextMeshProUGUI paramFriendlyFire;
        public TextMeshProUGUI paramGravity;
        public TextMeshProUGUI enabledModules;
        public CreateBattleScreenComponent createBattleScreen;
        public GameObject chat;

        public void AddUser(Entity userEntity, bool selfUser, bool customLobby)
        {
            TeamColor teamColor = userEntity.GetComponent<TeamColorComponent>().TeamColor;
            if (!this.teamBattleMode)
            {
                this.AddUserToFirstList(userEntity, selfUser, customLobby);
            }
            else if (teamColor == TeamColor.RED)
            {
                this.AddUserToSecondList(userEntity, selfUser, customLobby);
            }
            else
            {
                this.AddUserToFirstList(userEntity, selfUser, customLobby);
            }
            this.UpdateInviteFriendsButton();
        }

        private void AddUserToFirstList(Entity userEntity, bool selfUser, bool customLobby)
        {
            this.redTeamList.RemoveUser(userEntity);
            this.blueTeamList.AddUser(userEntity, selfUser, customLobby);
        }

        private void AddUserToSecondList(Entity userEntity, bool selfUser, bool customLobby)
        {
            this.blueTeamList.RemoveUser(userEntity);
            this.redTeamList.AddUser(userEntity, selfUser, customLobby);
        }

        public void CleanUsersList()
        {
            this.blueTeamList.Clean();
            this.redTeamList.Clean();
            this.UpdateInviteFriendsButton();
        }

        private List<VisualItem> FilterItemsList(List<VisualItem> items)
        {
            List<VisualItem> list = new List<VisualItem>();
            foreach (VisualItem item in items)
            {
                if ((item.UserItem != null) && !item.WaitForBuy)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public void InitHullDropDowns()
        {
            List<VisualItem> items = this.FilterItemsList(this.Hull.Skins.ToList<VisualItem>());
            List<VisualItem> list2 = this.FilterItemsList(GarageItemsRegistry.Paints.ToList<VisualItem>());
            this.hullSkinsDropDownList.UpdateList(items);
            this.hullPaintDropDownList.UpdateList(list2);
        }

        public void InitPresetsDropDown(List<PresetItem> items)
        {
            this.presetsDropDownList.UpdateList(items);
        }

        public void InitTurretDropDowns()
        {
            List<VisualItem> items = this.FilterItemsList(this.Turret.Skins.ToList<VisualItem>());
            List<VisualItem> list2 = this.FilterItemsList(GarageItemsRegistry.Coatings.ToList<VisualItem>());
            this.turretSkinsDropDownList.UpdateList(items);
            this.turretPaintDropDownList.UpdateList(list2);
        }

        private void Mount(Entity target)
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<MountPresetEvent>(target);
        }

        private void OnDisable()
        {
            this.presetsDropDownList.onDropDownListItemSelected -= new OnDropDownListItemSelected(this.OnPresetSelected);
        }

        private void OnEnable()
        {
            this.presetsDropDownList.onDropDownListItemSelected += new OnDropDownListItemSelected(this.OnPresetSelected);
            this.SendEvent<MatchLobbyShowEvent>();
        }

        public void OnPresetSelected(ListItem item)
        {
            PresetItem data = (PresetItem) item.Data;
            this.Mount(data.presetEntity);
        }

        public void OnVisualItemSelected(ListItem item)
        {
            VisualItem data = (VisualItem) item.Data;
            this.Mount(data.UserItem);
        }

        public void RemoveUser(Entity userEntity)
        {
            this.blueTeamList.RemoveUser(userEntity);
            this.redTeamList.RemoveUser(userEntity);
            this.UpdateInviteFriendsButton();
        }

        public void SetHullLabels()
        {
            this.hullName.text = this.Hull.Name;
            this.hullFeature.text = this.Hull.Feature;
        }

        public void SetMapPreview(Texture2D image)
        {
            this.mapIcon.color = Color.white;
            this.mapIcon.sprite = Sprite.Create(image, new Rect(Vector2.zero, new Vector2((float) image.width, (float) image.height)), Vector2.one * 0.5f);
        }

        public void SetTeamBattleMode(bool teamBattleMode, int teamLimit, int userLimit)
        {
            this.teamBattleMode = teamBattleMode;
            this.redTeamListWithHeader.gameObject.SetActive(teamBattleMode);
            this.teamList1Title.SetActive(teamBattleMode);
            if (!teamBattleMode)
            {
                this.blueTeamList.MaxCount = userLimit;
            }
            else
            {
                this.blueTeamList.MaxCount = teamLimit;
                this.redTeamList.MaxCount = teamLimit;
            }
        }

        public void SetTurretLabels()
        {
            this.turretName.text = this.Turret.Name;
            this.turretFeature.text = this.Turret.Feature;
        }

        public void ShowChat(bool show)
        {
            this.chat.SetActive(show);
        }

        public void ShowCustomLobbyElements(bool show)
        {
            this.customGameElements.SetActive(show);
        }

        public void ShowEditParamsButton(bool show, bool interactable)
        {
            this.editParamsButton.SetActive(show);
            this.editParamsButton.GetComponent<Button>().interactable = interactable;
            this.openBattleButton.SetActive(show);
            this.inviteFriendsButton.SetActive(show);
        }

        protected override void Subscribe()
        {
        }

        private void UpdateInviteFriendsButton()
        {
            bool flag = !this.teamBattleMode ? this.blueTeamList.HasEmptyCells() : (this.blueTeamList.HasEmptyCells() || this.redTeamList.HasEmptyCells());
            this.inviteFriendsButton.GetComponent<Button>().interactable = flag;
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem Hull { get; set; }

        public TankPartItem Turret { get; set; }

        public bool ShowSearchingText
        {
            set
            {
                this.blueTeamList.ShowSearchingText = value;
                this.redTeamList.ShowSearchingText = value;
            }
        }

        public string MapName
        {
            set => 
                this.mapTitle.text = value;
        }

        public string ModeName
        {
            set => 
                this.gameModeTitle.text = value;
        }

        public TeamColor UserTeamColor
        {
            set
            {
                this.blueTeamList.ShowJoinButton = value == TeamColor.RED;
                this.redTeamList.ShowJoinButton = value == TeamColor.BLUE;
            }
        }
    }
}

