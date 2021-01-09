namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class SquadTeammateInteractionTooltipContent : InteractionTooltipContent
    {
        [SerializeField]
        private Button profileButton;
        [SerializeField]
        private Button leaveSquadButton;
        [SerializeField]
        private Button removeFromSquadButton;
        [SerializeField]
        private Button giveLeaderButton;
        [SerializeField]
        private Button addFriendButton;
        [SerializeField]
        private Button friendRequestSentButton;
        [SerializeField]
        private Button changeAvatarButton;
        private Entity teammateEntity;
        public LocalizedField friendRequestResponce;

        public void AddToFriendsList()
        {
            if (this.teammateEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<RequestFriendSquadInternalEvent>(this.teammateEntity);
                base.ShowResponse(this.friendRequestResponce.Value);
            }
            base.Hide();
        }

        protected override void Awake()
        {
            base.Awake();
            this.profileButton.onClick.AddListener(new UnityAction(this.OpenProfile));
            this.leaveSquadButton.onClick.AddListener(new UnityAction(this.LeaveSquad));
            this.removeFromSquadButton.onClick.AddListener(new UnityAction(this.RemoveFromSquad));
            this.giveLeaderButton.onClick.AddListener(new UnityAction(this.GiveLeader));
            this.addFriendButton.onClick.AddListener(new UnityAction(this.AddToFriendsList));
            this.changeAvatarButton.onClick.AddListener(new UnityAction(this.ShowAvatarMenu));
        }

        public void GiveLeader()
        {
            if (this.teammateEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<ChangeSquadLeaderInternalEvent>(this.teammateEntity);
            }
            base.Hide();
        }

        public override void Init(object data)
        {
            SquadTeammateInteractionTooltipContentData data2 = (SquadTeammateInteractionTooltipContentData) data;
            this.teammateEntity = data2.teammateEntity;
            this.changeAvatarButton.gameObject.SetActive(!data2.ShowProfileButton);
            this.profileButton.gameObject.SetActive(data2.ShowProfileButton);
            this.leaveSquadButton.gameObject.SetActive(data2.ShowLeaveSquadButton);
            this.removeFromSquadButton.gameObject.SetActive(data2.ShowRemoveFromSquadButton);
            this.removeFromSquadButton.interactable = data2.ActiveRemoveFromSquadButton;
            this.giveLeaderButton.gameObject.SetActive(data2.ShowGiveLeaderButton);
            this.giveLeaderButton.interactable = data2.ActiveGiveLeaderButton;
            this.addFriendButton.gameObject.SetActive(data2.ShowAddFriendButton);
            this.friendRequestSentButton.gameObject.SetActive(data2.ShowFriendRequestSentButton);
        }

        public void LeaveSquad()
        {
            if (this.teammateEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<LeaveSquadInternalEvent>(this.teammateEntity);
            }
            base.Hide();
        }

        public void OpenProfile()
        {
            if (this.teammateEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new ShowProfileScreenEvent(this.teammateEntity.Id), this.teammateEntity);
            }
            base.Hide();
        }

        public void RemoveFromSquad()
        {
            if (this.teammateEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<KickOutFromSquadInternalEvent>(this.teammateEntity);
            }
            base.Hide();
        }

        public void ShowAvatarMenu()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<AvatarMenuSystem.ShowMenuEvent>(new EntityStub());
            base.Hide();
        }
    }
}

