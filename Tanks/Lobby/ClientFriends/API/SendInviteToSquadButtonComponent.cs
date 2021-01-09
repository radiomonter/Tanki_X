namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class SendInviteToSquadButtonComponent : BehaviourComponent
    {
        public void AttachToUserGroup()
        {
            base.GetComponentInParent<UserLabelComponent>().GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(base.GetComponent<EntityBehaviour>().Entity);
        }
    }
}

