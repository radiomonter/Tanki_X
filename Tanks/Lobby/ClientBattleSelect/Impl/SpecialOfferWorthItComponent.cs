namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class SpecialOfferWorthItComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI worthItText;
        [SerializeField]
        private LocalizedField worthItLocalizedField;

        public void SetLabel(int labelPercentage)
        {
            if (labelPercentage <= 0)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                this.worthItText.text = string.Format(this.worthItLocalizedField.Value, labelPercentage);
            }
        }
    }
}

