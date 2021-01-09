namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class MostPlayedEquipmentUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI turretIcon;
        [SerializeField]
        private TextMeshProUGUI turretLabel;
        [SerializeField]
        private TextMeshProUGUI hullIcon;
        [SerializeField]
        private TextMeshProUGUI hullLabel;
        [SerializeField]
        private GameObject content;

        private void SetIcon(TextMeshProUGUI t, string s)
        {
            t.text = "<sprite name=\"" + s + "\">";
        }

        public void SetMostPlayed(string turretUID, string turretName, string hullUID, string hullName)
        {
            this.SetIcon(this.turretIcon, turretUID);
            this.SetIcon(this.hullIcon, hullUID);
            this.turretLabel.text = turretName;
            this.hullLabel.text = hullName;
        }

        public void SwitchState(bool enabled)
        {
            this.content.SetActive(enabled);
            base.GetComponent<LayoutElement>().preferredWidth = !enabled ? ((float) 0) : ((float) 310);
        }
    }
}

