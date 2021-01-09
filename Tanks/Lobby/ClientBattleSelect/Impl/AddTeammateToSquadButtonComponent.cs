namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class AddTeammateToSquadButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private RectTransform popupPosition;

        public Vector3 PopupPosition =>
            this.popupPosition.position;
    }
}

