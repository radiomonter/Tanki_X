namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class InviteFriendsButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private RectTransform popupPosition;

        public Vector3 PopupPosition =>
            this.popupPosition.position;
    }
}

