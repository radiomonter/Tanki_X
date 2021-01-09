namespace Tanks.Lobby.ClientQuests.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class InBattleQuestItemGUIRewardComponent : MonoBehaviour
    {
        [SerializeField]
        private TankPartItemIcon itemIcon;
        [SerializeField]
        private TextMeshProUGUI quantityText;

        public void SetReward(int quantity)
        {
            this.quantityText.text = quantity.ToString();
        }

        public void SetReward(int quantity, long itemId)
        {
            this.SetReward(quantity);
            this.itemIcon.SetIconWithName(itemId.ToString());
        }
    }
}

