namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class Sprite3D : MonoBehaviour
    {
        private bool currentMaterialInstance;
        private static Mesh _planeMesh;
        protected Material assetMaterial;
        private Material instanceMaterial;
        public Material material;
        public float width = 100f;
        public float height = 100f;
        public float scale = 1f;
        public float originX = 0.5f;
        public float originY = 0.5f;
        public bool receiveShadows = true;
        public bool castShadows = true;
        public bool useOwnRotation;
        public bool useInstanceMaterial;
        public float offsetToCamera;
        public float minDistanceToCamera;
        private Camera _cachedCamera;

        protected void Awake()
        {
            this.assetMaterial = this.material;
            this.UpdateMaterial();
        }

        private static Mesh CreatePlane()
        {
            Mesh mesh2 = new Mesh();
            mesh2.vertices = new Vector3[] { new Vector3(50f, -50f, 0f), new Vector3(-50f, -50f, 0f), new Vector3(-50f, 50f, 0f), new Vector3(50f, 50f, 0f) };
            mesh2.triangles = new int[] { 0, 3, 2, 1, 0, 2 };
            mesh2.uv = new Vector2[] { new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(0f, 1f) };
            Mesh mesh = mesh2;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public virtual void Draw()
        {
            if (this.CachedCamera)
            {
                Matrix4x4 matrixx = new Matrix4x4 {
                    m00 = 1f,
                    m11 = 1f,
                    m22 = 1f,
                    m33 = 1f,
                    m03 = 100f * (this.originX - 0.5f),
                    m13 = 100f * (this.originY - 0.5f)
                };
                Quaternion q = !this.useOwnRotation ? Quaternion.LookRotation(-this._cachedCamera.transform.forward) : base.gameObject.transform.rotation;
                Vector3 s = new Vector3((this.scale * this.width) / 100f, (this.scale * this.height) / 100f, 1f);
                Matrix4x4 matrixx3 = new Matrix4x4();
                Vector3 vector2 = this._cachedCamera.transform.position - base.transform.position;
                float b = Mathf.Max((float) 0f, (float) (vector2.magnitude - this.minDistanceToCamera));
                b = Mathf.Min(this.offsetToCamera, b);
                matrixx3.SetTRS(base.transform.position + (vector2.normalized * b), q, s);
                this.UpdateMaterialIfNeeded();
                Graphics.DrawMesh(_planeMesh, matrixx3 * matrixx, this.material, base.gameObject.layer, null, 0, null, this.castShadows, this.receiveShadows);
            }
        }

        private void LateUpdate()
        {
            this.Draw();
        }

        protected void OnDestroy()
        {
            if (this.useInstanceMaterial)
            {
                Destroy(this.instanceMaterial);
            }
        }

        protected void Start()
        {
            _planeMesh = CreatePlane();
        }

        protected void UpdateMaterial()
        {
            this.currentMaterialInstance = this.useInstanceMaterial;
            if (this.useInstanceMaterial)
            {
                this.instanceMaterial = Instantiate<Material>(this.assetMaterial);
                this.material = this.instanceMaterial;
            }
            else
            {
                this.material = this.assetMaterial;
                if (this.instanceMaterial != null)
                {
                    Destroy(this.instanceMaterial);
                }
            }
        }

        protected void UpdateMaterialIfNeeded()
        {
            if (this.currentMaterialInstance != this.useInstanceMaterial)
            {
                this.UpdateMaterial();
            }
        }

        public Camera CachedCamera
        {
            get
            {
                if (!this._cachedCamera)
                {
                    this._cachedCamera = Camera.main;
                }
                return this._cachedCamera;
            }
        }
    }
}

