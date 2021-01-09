namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class TeleportHeaderView : MonoBehaviour
    {
        public LocalizedField lvl1;
        public LocalizedField lvl2;
        public LocalizedField lvl3;
        public LocalizedField lvl4;
        public LocalizedField lvl5;
        public LocalizedField broken;
        public LocalizedField hint;
        public LocalizedField brokenHint;
        public TextMeshProUGUI labelText;
        public TextMeshProUGUI hintText;
        private List<LocalizedField> labels;

        private void Awake()
        {
            List<LocalizedField> list = new List<LocalizedField> {
                this.lvl1,
                this.lvl2,
                this.lvl3,
                this.lvl4,
                this.lvl5
            };
            this.labels = list;
        }

        public void SetBrokenView()
        {
            this.labelText.text = this.broken.Value.ToUpper();
            this.hintText.text = (string) this.brokenHint;
        }

        public void UpdateView(int zoneIndex)
        {
            zoneIndex = Math.Min(zoneIndex, this.labels.Count - 1);
            this.labelText.text = this.labels[zoneIndex].Value.ToUpper();
            this.hintText.text = (string) this.hint;
        }
    }
}

