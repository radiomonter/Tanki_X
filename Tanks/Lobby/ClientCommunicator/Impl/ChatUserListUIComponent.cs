namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientCommunicator.API;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ChatUserListUIComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI emptyFriendsListLabel;
        [SerializeField]
        private LocalizedField noFriendsOnlineText;
        [SerializeField]
        private TMP_InputField searchingInput;
        [SerializeField]
        private float inputDelayInSec;
        [SerializeField]
        private ChatUserListUITableView tableView;
        private List<UserCellData> participants = new List<UserCellData>();
        private List<UserCellData> pending = new List<UserCellData>();
        private List<UserCellData> friends = new List<UserCellData>();
        private bool friendsLoaded;
        private bool participantsLoaded;
        private bool pendingLoaded;
        private bool inputChanged;
        private float lastChangeTime;
        private ChatUserListShowMode showMode;
        [SerializeField]
        private ChatUserListShowMode defaultShowMode;
        [SerializeField]
        private Button PartipientsButton;
        [SerializeField]
        private Button FriendsButton;

        public void AddFriend(Dictionary<long, string> FriendsIdsAndNicknames)
        {
            foreach (long num in FriendsIdsAndNicknames.Keys)
            {
                UserCellData item = new UserCellData(num, FriendsIdsAndNicknames[num]);
                this.friends.Add(item);
            }
            this.friendsLoaded = true;
            this.ShowMode = this.showMode;
        }

        public void AddFriend(long userId, string userUid)
        {
            UserCellData item = new UserCellData(userId, userUid);
            this.friends.Add(item);
            this.ShowMode = this.showMode;
        }

        public void AddFriends(Dictionary<long, string> FriendsIdsAndNicknames)
        {
            foreach (long num in FriendsIdsAndNicknames.Keys)
            {
                UserCellData item = new UserCellData(num, FriendsIdsAndNicknames[num]);
                this.friends.Add(item);
            }
            this.friendsLoaded = true;
            this.ShowMode = this.showMode;
        }

        public void AddParticipants()
        {
        }

        private void Awake()
        {
            if (this.PartipientsButton != null)
            {
                this.PartipientsButton.onClick.AddListener(new UnityAction(this.ShowParticipants));
            }
            if (this.FriendsButton != null)
            {
                this.FriendsButton.onClick.AddListener(new UnityAction(this.ShowFriends));
            }
        }

        private void CheckContentVisibility()
        {
            if (this.tableView.Items.Count != 0)
            {
                if (this.emptyFriendsListLabel.gameObject.activeSelf)
                {
                    this.emptyFriendsListLabel.gameObject.SetActive(false);
                }
            }
            else
            {
                string str = string.Empty;
                str = this.noFriendsOnlineText.Value;
                if (!this.emptyFriendsListLabel.gameObject.activeSelf || (this.emptyFriendsListLabel.text != str))
                {
                    this.emptyFriendsListLabel.text = str;
                    this.emptyFriendsListLabel.gameObject.SetActive(true);
                }
            }
        }

        public int GetUserDataIndexById(long userId, List<UserCellData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == userId)
                {
                    return i;
                }
            }
            return -1;
        }

        public void InputUpdate()
        {
            if (this.inputChanged && ((UnityTime.time - this.lastChangeTime) > this.inputDelayInSec))
            {
                this.UpdateFilterString();
                this.inputChanged = false;
            }
        }

        private void OnDisable()
        {
            this.pending.Clear();
            this.participants.Clear();
            this.friends.Clear();
            this.friendsLoaded = false;
            this.searchingInput.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.searchingInput.text = string.Empty;
            this.searchingInput.scrollSensitivity = 0f;
            this.searchingInput.Select();
            this.searchingInput.onValueChanged.AddListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
            this.ShowMode = this.defaultShowMode;
        }

        private void OnSearchingInputValueChanged(string value)
        {
            this.inputChanged = true;
            this.lastChangeTime = UnityTime.time;
        }

        public void RemoveFriend(long userId)
        {
            <RemoveFriend>c__AnonStorey1 storey = new <RemoveFriend>c__AnonStorey1 {
                userId = userId
            };
            this.friends.RemoveAll(new Predicate<UserCellData>(storey.<>m__0));
            this.ShowMode = this.showMode;
        }

        public void ShowFriends()
        {
            this.ShowMode = ChatUserListShowMode.Invite;
        }

        public void ShowParticipants()
        {
            this.ShowMode = ChatUserListShowMode.Participants;
        }

        private void Update()
        {
            if (this.loaded)
            {
                this.CheckContentVisibility();
                this.InputUpdate();
            }
        }

        private void UpdateFilterString()
        {
            this.tableView.FilterString = this.searchingInput.text;
        }

        public void UpdateTable(List<UserCellData> items)
        {
            this.tableView.Items = items;
            this.tableView.FilterString = this.tableView.FilterString;
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        private bool loaded =>
            (this.friendsLoaded && this.participantsLoaded) && this.pendingLoaded;

        public ChatUserListShowMode ShowMode
        {
            get => 
                this.showMode;
            set
            {
                this.showMode = value;
                List<UserCellData> items = new List<UserCellData>();
                ChatUserListShowMode showMode = this.showMode;
                if (showMode == ChatUserListShowMode.Participants)
                {
                    this.PartipientsButton.GetComponent<Animator>().SetBool("activated", true);
                    this.FriendsButton.GetComponent<Animator>().SetBool("activated", false);
                    items.AddRange(this.participants);
                }
                else if (showMode == ChatUserListShowMode.Invite)
                {
                    this.PartipientsButton.GetComponent<Animator>().SetBool("activated", false);
                    this.FriendsButton.GetComponent<Animator>().SetBool("activated", true);
                    items.AddRange(this.friends.Where<UserCellData>(delegate (UserCellData x) {
                        <>c__AnonStorey0 storey = new <>c__AnonStorey0 {
                            x = x
                        };
                        return !this.participants.Exists(new Predicate<UserCellData>(storey.<>m__0)) && !this.pending.Exists(new Predicate<UserCellData>(storey.<>m__1));
                    }));
                }
                this.UpdateTable(items);
            }
        }

        [CompilerGenerated]
        private sealed class <>c__AnonStorey0
        {
            internal UserCellData x;

            internal bool <>m__0(UserCellData p) => 
                p.id == this.x.id;

            internal bool <>m__1(UserCellData p) => 
                p.id == this.x.id;
        }

        [CompilerGenerated]
        private sealed class <RemoveFriend>c__AnonStorey1
        {
            internal long userId;

            internal bool <>m__0(UserCellData x) => 
                x.id == this.userId;
        }
    }
}

