namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using TMPro;
    using UnityEngine;

    public class EnergyCompensationDialog : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI quantiumsCount;
        [SerializeField]
        private TextMeshProUGUI crysCount;

        public void Show(long charges, long crys, List<Animator> animators = null)
        {
            this.quantiumsCount.text = "x" + charges;
            this.crysCount.text = "x" + crys;
            foreach (TextMeshProUGUI ougui in base.GetComponentsInChildren<TextMeshProUGUI>())
            {
                ougui.fontStyle = FontStyles.UpperCase;
            }
            base.Show(animators);
        }
    }
}

