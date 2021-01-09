namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class InviteFriendListItemComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject userLabelContainer;
        [SerializeField]
        private GameObject battleLabelContainer;
        [SerializeField]
        private GameObject notificationContainer;
        [SerializeField]
        private Text notificationText;

        public GameObject UserLabelContainer =>
            this.userLabelContainer;

        public GameObject BattleLabelContainer =>
            this.battleLabelContainer;

        public GameObject NotificationContainer =>
            this.notificationContainer;

        public Text NotificationText =>
            this.notificationText;
    }
}

