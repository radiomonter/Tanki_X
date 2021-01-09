namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class HUDWorldSpaceCanvas : MonoBehaviour, Component
    {
        public Canvas canvas;
        public GameObject nameplatePrefab;
        public Vector3 offset;
        [SerializeField]
        private GameObject damageInfoPrefab;

        public GameObject DamageInfoPrefab =>
            this.damageInfoPrefab;
    }
}

