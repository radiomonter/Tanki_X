namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class AmbientMapSoundEffectMarkerComponent : BehaviourComponent
    {
        [SerializeField]
        private Tanks.Battle.ClientGraphics.API.AmbientSoundFilter ambientSoundFilter;

        public Tanks.Battle.ClientGraphics.API.AmbientSoundFilter AmbientSoundFilter =>
            this.ambientSoundFilter;
    }
}

