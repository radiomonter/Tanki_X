namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class RailgunTrailComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private GameObject tipPrefab;

        public GameObject Prefab =>
            this.prefab;

        public GameObject TipPrefab =>
            this.tipPrefab;
    }
}

