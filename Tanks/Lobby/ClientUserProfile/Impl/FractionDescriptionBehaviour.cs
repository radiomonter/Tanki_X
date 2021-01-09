namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class FractionDescriptionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _fractionTitle;
        [SerializeField]
        private TextMeshProUGUI _fractionSlogan;
        [SerializeField]
        private TextMeshProUGUI _fractionDescription;
        [SerializeField]
        private ImageSkin _fractionLogo;
        [SerializeField]
        private FractionButtonComponent[] _fractionButtons;

        public string FractionTitle
        {
            set => 
                this._fractionTitle.text = value;
        }

        public string FractionSlogan
        {
            set => 
                this._fractionSlogan.text = value;
        }

        public string FractionDescription
        {
            set => 
                this._fractionDescription.text = value;
        }

        public string LogoUid
        {
            set => 
                this._fractionLogo.SpriteUid = value;
        }

        public Entity FractionId
        {
            set
            {
                foreach (FractionButtonComponent component in this._fractionButtons)
                {
                    component.FractionEntity = value;
                }
            }
        }
    }
}

