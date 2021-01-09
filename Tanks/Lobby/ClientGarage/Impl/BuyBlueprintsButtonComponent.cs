namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class BuyBlueprintsButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private LocalizedField buyBlueprintButtonLocalizedField;

        public bool mountButtonActive
        {
            set
            {
                base.GetComponent<Button>().interactable = value;
                base.GetComponent<CanvasGroup>().alpha = !value ? 0.2f : 1f;
            }
        }
    }
}

