namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class AmbientZoneSoundEffectMarkerComponent : BehaviourComponent
    {
        [SerializeField]
        private Tanks.Battle.ClientGraphics.Impl.AmbientZoneSoundEffect ambientZoneSoundEffect;

        public Tanks.Battle.ClientGraphics.Impl.AmbientZoneSoundEffect AmbientZoneSoundEffect =>
            this.ambientZoneSoundEffect;
    }
}

