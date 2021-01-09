namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LeagueUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI leagueName;
        [SerializeField]
        private ImageSkin leagueIcon;
        [SerializeField]
        private TextMeshProUGUI leaguePoints;
        [SerializeField]
        private LocalizedField pointsLocalizedField;

        public void SetLeague(string name, string icon, double points)
        {
            this.leagueName.text = name;
            this.leaguePoints.text = this.pointsLocalizedField.Value + "\n" + Math.Truncate(points);
            this.leagueIcon.SpriteUid = icon;
        }
    }
}

