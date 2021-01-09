namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class IncomingFriendButtonsComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject acceptButton;
        [SerializeField]
        private GameObject declineButton;
        private EntityBehaviour _mainEntityBehaviour;
        private EntityBehaviour _acceptEntityBehaviour;
        private EntityBehaviour _declineEntityBehaviour;

        private void OnEnable()
        {
            this._mainEntityBehaviour = base.GetComponent<EntityBehaviour>();
            this._acceptEntityBehaviour = this.acceptButton.GetComponent<EntityBehaviour>();
            this._declineEntityBehaviour = this.declineButton.GetComponent<EntityBehaviour>();
        }

        public bool IsIncoming
        {
            set
            {
                this.acceptButton.transform.parent.gameObject.SetActive(value);
                if (value)
                {
                    Entity entity = this._acceptEntityBehaviour.Entity;
                    entity.RemoveComponentIfPresent<UserGroupComponent>();
                    Entity entity2 = this._declineEntityBehaviour.Entity;
                    entity2.RemoveComponentIfPresent<UserGroupComponent>();
                    this._mainEntityBehaviour.Entity.GetComponent<UserGroupComponent>().Attach(entity).Attach(entity2);
                }
            }
        }
    }
}

