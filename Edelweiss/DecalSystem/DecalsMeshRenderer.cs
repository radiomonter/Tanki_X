namespace Edelweiss.DecalSystem
{
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.MeshFilter)), RequireComponent(typeof(UnityEngine.MeshRenderer))]
    public class DecalsMeshRenderer : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private UnityEngine.MeshFilter m_MeshFilter;
        [HideInInspector, SerializeField]
        private UnityEngine.MeshRenderer m_MeshRenderer;

        public UnityEngine.MeshFilter MeshFilter
        {
            get
            {
                if (this.m_MeshFilter == null)
                {
                    this.m_MeshFilter = base.GetComponent<UnityEngine.MeshFilter>();
                }
                return this.m_MeshFilter;
            }
        }

        public UnityEngine.MeshRenderer MeshRenderer
        {
            get
            {
                if (this.m_MeshRenderer == null)
                {
                    this.m_MeshRenderer = base.GetComponent<UnityEngine.MeshRenderer>();
                }
                return this.m_MeshRenderer;
            }
        }
    }
}

