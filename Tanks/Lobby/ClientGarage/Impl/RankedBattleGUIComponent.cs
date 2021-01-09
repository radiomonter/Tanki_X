namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class RankedBattleGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin leagueIcon;
        [SerializeField]
        private TextMeshProUGUI leagueName;
        [SerializeField]
        private LocalizedField costTextLocalization;

        public void Click()
        {
            base.GetComponent<Button>().onClick.Invoke();
        }

        public void SetLeague(string name, string icon)
        {
            this.leagueName.text = name;
            this.leagueIcon.SpriteUid = icon;
        }
    }
}

