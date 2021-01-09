namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public abstract class GenericDecalProjectorBaseComponent : MonoBehaviour
    {
        private Transform m_CachedTransform;
        public LayerMask affectedLayers = -1;
        public bool affectInactiveRenderers;
        public bool affectOtherDecals;
        public bool skipUnreadableMeshes;
        public DetailsMode detailsMode;
        public AffectedDetail[] affectedDetails = new AffectedDetail[0];
        public float cullingAngle = 90f;
        public float meshOffset;
        public bool projectAfterOffset;
        public float normalsSmoothing;
        public int uv1RectangleIndex;
        public int uv2RectangleIndex;
        public Color vertexColor = Color.white;
        [SerializeField]
        private float m_VertexColorBlending;

        protected GenericDecalProjectorBaseComponent()
        {
        }

        public GenericDecalsBase GetDecalsBase()
        {
            GenericDecalsBase component = null;
            for (Transform transform = base.transform; (transform != null) && (component == null); transform = transform.parent)
            {
                component = transform.GetComponent<GenericDecalsBase>();
            }
            return component;
        }

        private void OnEnable()
        {
            this.m_CachedTransform = base.GetComponent<Transform>();
        }

        private Matrix4x4 UnscaledLocalToWorldMatrix() => 
            Matrix4x4.TRS(this.CachedTransform.position, this.CachedTransform.rotation, Vector3.one);

        public Bounds WorldBounds()
        {
            Matrix4x4 matrixx = this.UnscaledLocalToWorldMatrix();
            Vector3 vector = (Vector3) (0.5f * this.CachedTransform.localScale);
            Vector3 vector2 = new Vector3(0f, -Mathf.Abs(vector.y), 0f);
            Bounds bounds = new Bounds(matrixx.MultiplyPoint3x4(Vector3.zero), Vector3.zero);
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, -vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(vector.x, -vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, vector.y, -vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, -vector.y, vector.z)));
            bounds.Encapsulate(matrixx.MultiplyPoint3x4(vector2 + new Vector3(-vector.x, -vector.y, -vector.z)));
            return bounds;
        }

        public Transform CachedTransform
        {
            get
            {
                if (this.m_CachedTransform == null)
                {
                    this.m_CachedTransform = base.transform;
                }
                return this.m_CachedTransform;
            }
        }

        public float VertexColorBlending
        {
            get => 
                this.m_VertexColorBlending;
            set
            {
                if ((value < 0f) || (value > 1f))
                {
                    throw new ArgumentOutOfRangeException("The blend value has to be in [0.0f, 1.0f].");
                }
                this.m_VertexColorBlending = value;
            }
        }
    }
}

