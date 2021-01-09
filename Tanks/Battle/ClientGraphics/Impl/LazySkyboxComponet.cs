namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class LazySkyboxComponet : BehaviourComponent
    {
        [SerializeField]
        private AssetReference skyBoxReference;

        public AssetReference SkyBoxReference =>
            this.skyBoxReference;
    }
}

