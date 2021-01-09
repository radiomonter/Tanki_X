namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;

    internal class UserInteractionsTooltipContent : ECSBehaviour, ITooltipContent
    {
        public GameObject addToFriendsButton;
        public GameObject muteButton;
        public GameObject reportButton;
        public GameObject copyNameButton;
        public GameObject responceMessagePrefab;
        public float displayTime = 10f;
        public LocalizedField requrestSendLocalization;
        public LocalizedField requestFriendshipLocalization;
        public LocalizedField muteStateLocalization;
        public LocalizedField unmuteStateLocalization;
        public LocalizedField addToFriendsResponce;
        public LocalizedField activateBlockResponce;
        public LocalizedField deactivateBlockResponce;
        public LocalizedField reportResponce;
        public LocalizedField copied;
        private long interactableUserId;
        private Entity selfUserEntity;
        private RectTransform rect;
        private string blockText;
        private InteractionSource interactionSource;
        private long sourceId;
        private string otherUserName;

        public void Awake()
        {
            this.rect = base.GetComponent<RectTransform>();
        }

        public void CopyName()
        {
            GUIUtility.systemCopyBuffer = this.otherUserName;
            this.ShowResponse(this.copied.Value);
            this.HideTooltip();
        }

        private void HideTooltip()
        {
            TooltipController.Instance.HideTooltip();
        }

        private void HideTooltipOnIdle()
        {
            if (this.PointerOutsideMenu())
            {
                this.HideTooltip();
            }
            else
            {
                base.Invoke("HideTooltipOnIdle", this.displayTime);
            }
        }

        public void Init(object data)
        {
            UserInteractionsData userData = (UserInteractionsData) data;
            this.selfUserEntity = userData.selfUserEntity;
            this.SetFriendshipButtonState(userData.canRequestFrendship, userData.friendshipRequestWasSend);
            this.blockText = !userData.isMuted ? this.activateBlockResponce.Value : this.deactivateBlockResponce.Value;
            this.interactableUserId = userData.userId;
            this.interactionSource = userData.interactionSource;
            this.sourceId = userData.sourceId;
            this.otherUserName = userData.OtherUserName;
            this.copyNameButton.SetActive(!string.IsNullOrEmpty(userData.OtherUserName));
            this.reportButton.SetActive(!userData.isReported);
            this.SetMuteButtonState(userData);
            base.Invoke("HideTooltipOnIdle", this.displayTime);
        }

        private bool PointerOutsideMenu() => 
            !RectTransformUtility.RectangleContainsScreenPoint(this.rect, Input.mousePosition);

        public void SendBlockUnblockRequest()
        {
            base.NewEvent(new ChangeBlockStateByUserIdRequest(this.interactableUserId, this.interactionSource, this.sourceId)).Attach(this.selfUserEntity).Schedule();
            this.ShowResponse(this.blockText);
            this.HideTooltip();
        }

        public void SendFriendRequest()
        {
            base.NewEvent(new RequestFriendshipByUserId(this.interactableUserId, this.interactionSource, this.sourceId)).Attach(this.selfUserEntity).Schedule();
            this.ShowResponse(this.addToFriendsResponce.Value);
            this.HideTooltip();
        }

        public void SendReportRequest()
        {
            base.NewEvent(new ReportUserByUserId(this.interactableUserId, this.interactionSource, this.sourceId)).Attach(this.selfUserEntity).Schedule();
            this.ShowResponse(this.reportResponce.Value);
            this.HideTooltip();
        }

        private void SetFriendshipButtonState(bool canRequestFriendship, bool friendshipRequestWasSend)
        {
            this.addToFriendsButton.SetActive(canRequestFriendship || friendshipRequestWasSend);
            TextMeshProUGUI componentInChildren = this.addToFriendsButton.GetComponentInChildren<TextMeshProUGUI>();
            componentInChildren.text = !canRequestFriendship ? (!friendshipRequestWasSend ? string.Empty : this.requrestSendLocalization.Value) : this.requestFriendshipLocalization.Value;
            componentInChildren.color = !friendshipRequestWasSend ? Color.white : Color.gray;
            this.addToFriendsButton.GetComponent<Button>().interactable = !friendshipRequestWasSend;
        }

        private void SetMuteButtonState(UserInteractionsData userData)
        {
            this.muteButton.GetComponentInChildren<TextMeshProUGUI>().text = !userData.isMuted ? this.muteStateLocalization.Value : this.unmuteStateLocalization.Value;
        }

        private void ShowResponse(string respondText)
        {
            GameObject obj2 = Instantiate<GameObject>(this.responceMessagePrefab);
            obj2.transform.SetParent(base.transform.parent.parent, false);
            obj2.GetComponent<RectTransform>().position = Input.mousePosition;
            obj2.GetComponentInChildren<TextMeshProUGUI>().text = respondText;
            obj2.SetActive(true);
            Destroy(obj2, obj2.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }

        public void Update()
        {
            bool keyUp = Input.GetKeyUp(KeyCode.Tab);
            bool flag3 = Input.GetKeyUp(KeyCode.Escape);
            if (((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && this.PointerOutsideMenu()) || (keyUp || flag3))
            {
                this.HideTooltip();
            }
        }
    }
}

