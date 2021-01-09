namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class ModulesNotificationBadgeComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private LocalizedField newModuleAvailable;
        [SerializeField]
        private LocalizedField moduleUpgradeAvailable;
        private State currentState;
        public TankPartModuleType TankPart;

        public State CurrentState
        {
            get => 
                this.currentState;
            set
            {
                this.text.gameObject.SetActive(false);
                this.text.text = "<color=#F2A00CFF>";
                this.currentState = value;
                State currentState = this.currentState;
                if (currentState == State.NewModuleAvailable)
                {
                    this.text.text = this.text.text + this.newModuleAvailable.Value;
                    this.text.gameObject.SetActive(true);
                }
                else if (currentState == State.ModuleUpgradeAvailable)
                {
                    this.text.text = this.text.text + this.moduleUpgradeAvailable.Value;
                    this.text.gameObject.SetActive(true);
                }
            }
        }
    }
}

