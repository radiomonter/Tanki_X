namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class FriendsListUIComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI emptyFriendsListLabel;
        [SerializeField]
        private LocalizedField noFriendsOnlineText;
        [SerializeField]
        private LocalizedField noFriendsText;
        [SerializeField]
        private LocalizedField noFriendsIncomingText;
        [SerializeField]
        private GameObject addAllButton;
        [SerializeField]
        private GameObject rejectAllButton;
        [SerializeField]
        private TMP_InputField searchingInput;
        [SerializeField]
        private float inputDelayInSec;
        public FriendsUITableView tableView;
        private List<UserCellData> incoming = new List<UserCellData>();
        private List<UserCellData> accepted = new List<UserCellData>();
        private List<UserCellData> outgoing = new List<UserCellData>();
        private bool loaded;
        private bool inputChanged;
        private float lastChangeTime;
        private FriendsShowMode showMode;
        [SerializeField]
        private FriendsShowMode defaultShowMode;
        [SerializeField]
        private Button AllFriendsButton;
        [SerializeField]
        private Button IncomnigFriendsButton;

        private void ActivateInputField()
        {
            this.searchingInput.ActivateInputField();
        }

        public void AddFriends(Dictionary<long, string> FriendsIdsAndNicknames, FriendType friendType)
        {
            foreach (long num in FriendsIdsAndNicknames.Keys)
            {
                this.AddItem(num, FriendsIdsAndNicknames[num], friendType);
            }
            this.loaded = true;
            this.ShowMode = this.showMode;
        }

        public void AddItem(long userId, string userUid, FriendType friendType)
        {
            UserCellData item = new UserCellData(userId, userUid);
            if (friendType == FriendType.Incoming)
            {
                this.incoming.Add(item);
            }
            else if (friendType != FriendType.Outgoing)
            {
                this.accepted.Add(item);
            }
            else
            {
                this.outgoing.Add(item);
            }
        }

        private void Awake()
        {
            if (this.AllFriendsButton != null)
            {
                this.AllFriendsButton.onClick.AddListener(new UnityAction(this.ShowAcceptedAndOutgoingFriends));
            }
            if (this.IncomnigFriendsButton != null)
            {
                this.IncomnigFriendsButton.onClick.AddListener(new UnityAction(this.ShowIncomingFriends));
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
                FriendsShowMode showMode = this.ShowMode;
                str = (showMode == FriendsShowMode.AcceptedAndOutgoing) ? this.noFriendsText.Value : ((showMode == FriendsShowMode.Incoming) ? this.noFriendsIncomingText.Value : this.noFriendsOnlineText.Value);
                if (!this.emptyFriendsListLabel.gameObject.activeSelf || (this.emptyFriendsListLabel.text != str))
                {
                    this.emptyFriendsListLabel.text = str;
                    this.emptyFriendsListLabel.gameObject.SetActive(true);
                }
            }
        }

        public void ClearIncoming(bool moveToAccepted)
        {
            List<UserCellData> list = new List<UserCellData>(this.incoming);
            this.incoming.Clear();
            foreach (UserCellData data in list)
            {
                this.tableView.RemoveUser(data.id, !moveToAccepted);
                if (moveToAccepted)
                {
                    this.AddItem(data.id, data.uid, FriendType.Accepted);
                }
            }
        }

        public void DisableAddAllButton()
        {
            this.addAllButton.SetActive(true);
        }

        public void DisableRejectAllButton()
        {
            this.rejectAllButton.SetActive(true);
        }

        public void EnableAddAllButton()
        {
            this.addAllButton.SetActive(true);
            this.rejectAllButton.SetActive(false);
        }

        public void EnableRejectAllButton()
        {
            this.addAllButton.SetActive(false);
            this.rejectAllButton.SetActive(true);
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

        private void ItemClearDone(UserListItemComponent item)
        {
            item.transform.SetParent(null, false);
            Destroy(item.gameObject);
        }

        private void OnDisable()
        {
            base.CancelInvoke();
            this.incoming.Clear();
            this.accepted.Clear();
            this.outgoing.Clear();
            this.loaded = false;
            this.searchingInput.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.searchingInput.text = string.Empty;
            this.searchingInput.scrollSensitivity = 0f;
            this.searchingInput.onValueChanged.AddListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
            this.ShowMode = this.defaultShowMode;
            base.Invoke("ActivateInputField", 0.5f);
        }

        private void OnSearchingInputValueChanged(string value)
        {
            this.inputChanged = true;
            this.lastChangeTime = UnityTime.time;
        }

        public void RemoveItem(long userId, bool toRight)
        {
            int userDataIndexById = this.GetUserDataIndexById(userId, this.incoming);
            if (userDataIndexById != -1)
            {
                this.incoming.RemoveAt(userDataIndexById);
            }
            else
            {
                userDataIndexById = this.GetUserDataIndexById(userId, this.accepted);
                if (userDataIndexById != -1)
                {
                    this.accepted.RemoveAt(userDataIndexById);
                }
                else
                {
                    userDataIndexById = this.GetUserDataIndexById(userId, this.outgoing);
                    if (userDataIndexById != -1)
                    {
                        this.outgoing.RemoveAt(userDataIndexById);
                    }
                }
            }
            this.tableView.RemoveUser(userId, toRight);
        }

        public void ResetButtons()
        {
            if ((this.addAllButton != null) && (this.rejectAllButton != null))
            {
                this.addAllButton.SetActive(false);
                this.rejectAllButton.SetActive(false);
            }
        }

        public void ShowAcceptedAndOutgoingFriends()
        {
            this.ShowMode = FriendsShowMode.AcceptedAndOutgoing;
        }

        public void ShowIncomingFriends()
        {
            this.ShowMode = FriendsShowMode.Incoming;
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

        public FriendsShowMode ShowMode
        {
            get => 
                this.showMode;
            set
            {
                this.showMode = value;
                List<UserCellData> items = new List<UserCellData>();
                FriendsShowMode showMode = this.ShowMode;
                if (showMode == FriendsShowMode.AcceptedAndOutgoing)
                {
                    this.AllFriendsButton.GetComponent<Animator>().SetBool("activated", true);
                    this.IncomnigFriendsButton.GetComponent<Animator>().SetBool("activated", false);
                    items.AddRange(this.accepted);
                    items.AddRange(this.outgoing);
                }
                else if (showMode != FriendsShowMode.Incoming)
                {
                    items.AddRange(this.accepted);
                }
                else
                {
                    this.AllFriendsButton.GetComponent<Animator>().SetBool("activated", false);
                    this.IncomnigFriendsButton.GetComponent<Animator>().SetBool("activated", true);
                    items.AddRange(this.incoming);
                }
                this.UpdateTable(items);
                this.ResetButtons();
            }
        }
    }
}

