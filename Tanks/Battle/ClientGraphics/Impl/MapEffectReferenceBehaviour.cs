namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientResources.API;
    using UnityEngine;

    public class MapEffectReferenceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AssetReference mapEffect;

        public AssetReference MapEffect =>
            this.mapEffect;
    }
}

