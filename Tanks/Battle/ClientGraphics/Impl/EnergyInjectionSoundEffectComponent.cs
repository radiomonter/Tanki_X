namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class EnergyInjectionSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private SoundController sound;

        public SoundController Sound =>
            this.sound;
    }
}

