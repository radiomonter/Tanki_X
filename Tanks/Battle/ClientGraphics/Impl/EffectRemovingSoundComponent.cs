namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class EffectRemovingSoundComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource sound;

        public AudioSource Sound =>
            this.sound;
    }
}

