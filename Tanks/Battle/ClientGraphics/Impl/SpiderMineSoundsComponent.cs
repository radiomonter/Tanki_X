namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class SpiderMineSoundsComponent : BehaviourComponent
    {
        [SerializeField]
        private SoundController runSoundController;

        public SoundController RunSoundController =>
            this.runSoundController;
    }
}

