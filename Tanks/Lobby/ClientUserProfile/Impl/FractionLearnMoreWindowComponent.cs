namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class FractionLearnMoreWindowComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _competitionTitle;
        [SerializeField]
        private TextMeshProUGUI _competitionDescription;
        [SerializeField]
        private ImageSkin _competitionLogo;

        public string CompetitionTitle
        {
            set => 
                this._competitionTitle.text = value;
        }

        public string CompetitionDescription
        {
            set => 
                this._competitionDescription.text = value;
        }

        public string CompetitionLogoUid
        {
            set => 
                this._competitionLogo.SpriteUid = value;
        }
    }
}

