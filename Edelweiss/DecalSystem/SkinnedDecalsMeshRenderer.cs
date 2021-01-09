namespace Edelweiss.DecalSystem
{
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.SkinnedMeshRenderer))]
    public class SkinnedDecalsMeshRenderer : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private UnityEngine.SkinnedMeshRenderer m_SkinnedMeshRenderer;

        public UnityEngine.SkinnedMeshRenderer SkinnedMeshRenderer
        {
            get
            {
                if (this.m_SkinnedMeshRenderer == null)
                {
                    this.m_SkinnedMeshRenderer = base.GetComponent<UnityEngine.SkinnedMeshRenderer>();
                }
                return this.m_SkinnedMeshRenderer;
            }
        }
    }
}

