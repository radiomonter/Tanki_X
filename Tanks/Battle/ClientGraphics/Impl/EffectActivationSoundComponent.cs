namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class EffectActivationSoundComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource sound;

        public AudioSource Sound =>
            this.sound;
    }
}

