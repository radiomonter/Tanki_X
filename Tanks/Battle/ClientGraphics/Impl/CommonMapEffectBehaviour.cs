namespace Tanks.Battle.ClientGraphics.Impl
{
    using UnityEngine;

    public class CommonMapEffectBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject commonMapEffectPrefab;

        public GameObject CommonMapEffectPrefab =>
            this.commonMapEffectPrefab;
    }
}

