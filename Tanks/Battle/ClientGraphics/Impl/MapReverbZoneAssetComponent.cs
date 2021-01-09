namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class MapReverbZoneAssetComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject mapReverbZonesRoot;

        public GameObject MapReverbZonesRoot =>
            this.mapReverbZonesRoot;
    }
}

