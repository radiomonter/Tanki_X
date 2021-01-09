namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;

    public class NumberOfBattlesLeftUIComponent : BehaviourComponent
    {
        public TextMeshProUGUI text;
        public LocalizedField numberOfBattlesText;

        public void DisplayBattlesLeft(int numberOfBattlesLeft)
        {
            this.text.text = $"{this.numberOfBattlesText.Value} {numberOfBattlesLeft}";
        }
    }
}

