namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ChestProgressBarComponent : BehaviourComponent
    {
        [SerializeField]
        private TooltipShowBehaviour tooltip;
        [SerializeField]
        private TooltipShowBehaviour chestTooltip;
        [SerializeField]
        private LocalizedField chestTooltipLocalization;
        [SerializeField]
        private LocalizedField chestTooltipLowLeagueLocalization;
        [SerializeField]
        private UIRectClipper bar;
        [SerializeField]
        private TextMeshProUGUI chestName;
        [SerializeField]
        private ImageSkin chestIcon;

        public void SetChest(string name, string imageUid)
        {
            this.chestName.text = name;
            this.chestIcon.SpriteUid = imageUid;
        }

        public void SetChestTooltip(long score, bool highLeague)
        {
            string str = string.Format(this.chestTooltipLocalization.Value, score);
            if (!highLeague)
            {
                str = str + this.chestTooltipLowLeagueLocalization.Value;
            }
            this.chestTooltip.TipText = str;
        }

        public void SetProgress(long current, long full)
        {
            this.bar.ToX = Mathf.Clamp01(((float) current) / ((float) full));
            this.tooltip.TipText = $"{current} / {full}";
        }
    }
}

