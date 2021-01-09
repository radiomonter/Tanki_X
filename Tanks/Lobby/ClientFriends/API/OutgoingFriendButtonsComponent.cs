namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class OutgoingFriendButtonsComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject revokeButton;

        public bool IsOutgoing
        {
            set
            {
                this.revokeButton.transform.parent.gameObject.SetActive(value);
                if (value)
                {
                    base.GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(this.revokeButton.GetComponent<EntityBehaviour>().Entity);
                }
            }
        }
    }
}

