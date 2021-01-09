namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class MapNativeSoundsAssetComponent : BehaviourComponent
    {
        [SerializeField]
        private MapNativeSoundsBehaviour asset;

        public MapNativeSoundsBehaviour Asset =>
            this.asset;
    }
}

