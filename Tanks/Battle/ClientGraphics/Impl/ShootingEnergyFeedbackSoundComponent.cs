namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShootingEnergyFeedbackSoundComponent : BehaviourComponent
    {
        [SerializeField]
        private WeaponFeedbackSoundBehaviour lowEnergyFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour LowEnergyFeedbackSoundAsset =>
            this.lowEnergyFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour Instance { get; set; }
    }
}

