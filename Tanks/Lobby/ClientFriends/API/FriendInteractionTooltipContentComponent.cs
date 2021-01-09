namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class FriendInteractionTooltipContentComponent : InteractionTooltipContent, AttachToEntityListener
    {
        [SerializeField]
        private Button profileButton;
        [SerializeField]
        private Button chatButton;
        [SerializeField]
        private Button enterAsSpectatorButton;
        [SerializeField]
        private Button removeButton;
        [SerializeField]
        private Button inviteToSquadButton;
        [SerializeField]
        private Button requestToSquadButton;
        [SerializeField]
        private Button requestToSquadWasSentButton;
        [SerializeField]
        private Button squadIsFullButton;
        private Entity friendEntity;
        public LocalizedField inviteToSquadResponce;
        public LocalizedField requestToSquadResponce;

        public void AttachedToEntity(Entity entity)
        {
            if (this.friendEntity != null)
            {
                this.friendEntity.GetComponent<UserGroupComponent>().Attach(base.GetComponent<EntityBehaviour>().Entity);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            this.profileButton.onClick.AddListener(new UnityAction(this.OpenProfile));
            this.chatButton.onClick.AddListener(new UnityAction(this.StartChat));
            this.removeButton.onClick.AddListener(new UnityAction(this.RemoveFriend));
            this.enterAsSpectatorButton.onClick.AddListener(new UnityAction(this.EnterAsSpectator));
            this.inviteToSquadButton.onClick.AddListener(new UnityAction(this.InviteToSquad));
            this.requestToSquadButton.onClick.AddListener(new UnityAction(this.RequestForInviteToSquad));
        }

        public void EnterAsSpectator()
        {
            if (this.friendEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<EnterAsSpectatorToFriendBattleEvent>(this.friendEntity);
            }
            base.Hide();
        }

        public override void Init(object data)
        {
            FriendInteractionTooltipData data2 = (FriendInteractionTooltipData) data;
            this.friendEntity = data2.FriendEntity;
            this.removeButton.gameObject.SetActive(data2.ShowRemoveButton);
            this.enterAsSpectatorButton.gameObject.SetActive(data2.ShowEnterAsSpectatorButton);
            this.inviteToSquadButton.gameObject.SetActive(data2.ShowInviteToSquadButton);
            this.inviteToSquadButton.interactable = data2.ActiveShowInviteToSquadButton;
            this.requestToSquadButton.gameObject.SetActive(data2.ShowRequestToSquadButton);
            this.chatButton.gameObject.SetActive(data2.ShowChatButton);
        }

        public void InviteToSquad()
        {
            if (this.friendEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new FriendInviteToSquadEvent(this.friendEntity.Id, InteractionSource.FRIENDS_LIST, 0L), this.friendEntity);
                base.ShowResponse(this.inviteToSquadResponce.Value);
            }
            base.Hide();
        }

        public void OpenProfile()
        {
            if (this.friendEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new ShowProfileScreenEvent(this.friendEntity.Id), this.friendEntity);
            }
            base.Hide();
        }

        public void RemoveFriend()
        {
            if (this.friendEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<RemoveFriendButtonEvent>(this.friendEntity);
            }
            base.Hide();
        }

        public void RequestForInviteToSquad()
        {
            if (this.friendEntity != null)
            {
                this.RequestToSquadWasSent();
                ECSBehaviour.EngineService.Engine.ScheduleEvent<RequestToSquadInternalEvent>(this.friendEntity);
            }
            base.Hide();
        }

        public void RequestToSquadWasSent()
        {
            this.requestToSquadButton.gameObject.SetActive(false);
            this.requestToSquadWasSentButton.gameObject.SetActive(true);
        }

        public void SquadIsFull()
        {
            if (this.requestToSquadButton.gameObject.activeInHierarchy || this.requestToSquadWasSentButton.gameObject.activeInHierarchy)
            {
                this.requestToSquadButton.gameObject.SetActive(false);
                this.requestToSquadWasSentButton.gameObject.SetActive(false);
                this.squadIsFullButton.gameObject.SetActive(true);
            }
        }

        public void StartChat()
        {
            if (this.friendEntity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new OpenPersonalChatFromContextMenuEvent(), this.friendEntity);
            }
            base.Hide();
        }
    }
}

