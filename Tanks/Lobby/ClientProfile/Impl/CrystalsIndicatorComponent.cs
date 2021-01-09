namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class CrystalsIndicatorComponent : BehaviourComponent
    {
        [SerializeField]
        private AnimatedLong number;

        public void SetValueWithoutAnimation(long value)
        {
            this.number.SetImmediate(value);
        }

        public long Value
        {
            get => 
                this.number.Value;
            set => 
                this.number.Value = value;
        }
    }
}

