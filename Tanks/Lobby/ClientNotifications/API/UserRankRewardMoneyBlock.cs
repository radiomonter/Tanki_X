namespace Tanks.Lobby.ClientNotifications.API
{
    using UnityEngine;
    using UnityEngine.UI;

    public class UserRankRewardMoneyBlock : MonoBehaviour
    {
        [SerializeField]
        private Text moneyRewardField;

        public Text MoneyRewardField =>
            this.moneyRewardField;
    }
}

