namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class FractionContainerComponent : BehaviourComponent
    {
        [SerializeField]
        private ImageSkin _fractionLogo;
        [SerializeField]
        private TextMeshProUGUI _fractionTitle;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        public FractionContainerTargets Target;

        public string FractionLogoUid
        {
            set => 
                this._fractionLogo.SpriteUid = value;
        }

        public string FractionTitle
        {
            set => 
                this._fractionTitle.text = value;
        }

        public Color FractionColor
        {
            set => 
                this._fractionTitle.color = value;
        }

        public bool IsAvailable
        {
            set => 
                this._canvasGroup.alpha = !value ? 0f : 1f;
        }

        public enum FractionContainerTargets
        {
            PLAYER_FRACTION,
            WINNER_FRACTION
        }
    }
}

