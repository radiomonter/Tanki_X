namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserRankRewardNotificationUnityComponent : BehaviourComponent
    {
        [SerializeField]
        private Text rankHeaderElement;
        [SerializeField]
        private Text rankNameElement;
        [SerializeField]
        private ImageListSkin rankImageSkin;
        [SerializeField]
        private UserRankRewardMoneyBlock xCrystalsBlock;
        [SerializeField]
        private UserRankRewardMoneyBlock crystalsBlock;

        public ImageListSkin RankImageSkin =>
            this.rankImageSkin;

        public Text RankHeaderElement =>
            this.rankHeaderElement;

        public Text RankNameElement =>
            this.rankNameElement;

        public UserRankRewardMoneyBlock XCrystalsBlock =>
            this.xCrystalsBlock;

        public UserRankRewardMoneyBlock CrystalsBlock =>
            this.crystalsBlock;
    }
}

