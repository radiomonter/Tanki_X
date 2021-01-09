namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class TutorialRewardsUIComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI crysCount;
        [SerializeField]
        private TextMeshProUGUI itemName;
        [SerializeField]
        private ImageSkin item;
        [SerializeField]
        private LocalizedField crysLocalizedField;
        [SerializeField]
        private LocalizedField itemLocalizedField;

        public void SetupTutorialReward(long crys, string itemSpriteUID)
        {
            this.crysCount.text = this.crysLocalizedField.Value + " x" + crys;
            this.itemName.text = this.itemLocalizedField.Value;
            this.item.SpriteUid = itemSpriteUID;
        }
    }
}

