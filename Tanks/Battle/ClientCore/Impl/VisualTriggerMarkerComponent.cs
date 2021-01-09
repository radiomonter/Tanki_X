namespace Tanks.Battle.ClientCore.Impl
{
    using UnityEngine;

    public class VisualTriggerMarkerComponent : MonoBehaviour
    {
        [SerializeField]
        private MeshCollider visualTriggerMeshCollider;

        public MeshCollider VisualTriggerMeshCollider =>
            this.visualTriggerMeshCollider;
    }
}

