namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class CompetitionAwardWindowComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _fractionNameText;
        [SerializeField]
        private TextMeshProUGUI _fractionRewardDescriptionText;
        [SerializeField]
        private ImageSkin _fractionRewardImage;

        public string FractionName
        {
            set => 
                this._fractionNameText.text = value;
        }

        public Color FractionColor
        {
            set => 
                this._fractionNameText.color = value;
        }

        public string FractionRewardDescription
        {
            set => 
                this._fractionRewardDescriptionText.text = value;
        }

        public string RewardImageUid
        {
            set => 
                this._fractionRewardImage.SpriteUid = value;
        }
    }
}

