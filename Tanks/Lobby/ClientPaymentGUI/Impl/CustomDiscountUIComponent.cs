namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;

    public class CustomDiscountUIComponent : BehaviourComponent
    {
        public TextMeshProUGUI description;

        public void OnDisable()
        {
            this.description.text = string.Empty;
        }
    }
}

