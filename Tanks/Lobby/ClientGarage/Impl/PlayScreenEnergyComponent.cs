namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PlayScreenEnergyComponent : BehaviourComponent
    {
        [SerializeField]
        private Color fullColor;
        [SerializeField]
        private Color partColor;
        private long maxEnergy;
        private long quantumCost;
        [SerializeField]
        private List<UIRectClipperY> energyBar;
        [SerializeField]
        private TextMeshProUGUI quantumCountText;
        [SerializeField]
        private TooltipShowBehaviour tooltip;

        public void Init(long maxEnergy, long cost)
        {
            this.maxEnergy = maxEnergy;
            this.quantumCost = cost;
        }

        public void SetEnergy(long currentEnergy)
        {
            long num = currentEnergy / this.quantumCost;
            for (int i = 0; i < this.energyBar.Count; i++)
            {
                if (i < num)
                {
                    this.energyBar[i].ToY = 1f;
                    this.energyBar[i].gameObject.GetComponent<Image>().color = this.fullColor;
                }
                else if (i != num)
                {
                    this.energyBar[i].ToY = 0f;
                }
                else
                {
                    this.energyBar[i].ToY = ((float) (currentEnergy - (num * this.quantumCost))) / ((float) this.quantumCost);
                    this.energyBar[i].gameObject.GetComponent<Image>().color = this.partColor;
                }
            }
            this.quantumCountText.text = "<size=25><sprite=0></size> " + num.ToString() + " / 3";
            this.tooltip.TipText = $"{currentEnergy} / {this.maxEnergy}";
        }
    }
}

