namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class DecalsTestBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Material decalMaterial;
        [SerializeField]
        private float projectDistantion = 100f;
        [SerializeField]
        private float projectSize = 1f;
        [SerializeField]
        private int hTilesCount = 1;
        [SerializeField]
        private int vTilesCount = 1;
        [SerializeField]
        private float mouseWheelSenetivity = 0.1f;
        [SerializeField]
        private int[] surfaceAtlasPositions = new int[5];
        [SerializeField]
        private bool updateEveryFrame = true;
        private Mesh decalMesh;
        private MeshFilter meshFilter;
        private Renderer renderer;
        private DecalMeshBuilder meshBuilder = new DecalMeshBuilder();
        private int counter;

        private void CreateMesh()
        {
            int num;
            GameObject obj2 = new GameObject("Decal Mesh");
            this.decalMesh = new Mesh();
            this.decalMesh.MarkDynamic();
            this.meshFilter = obj2.AddComponent<MeshFilter>();
            this.meshFilter.mesh = this.decalMesh;
            this.renderer = obj2.AddComponent<MeshRenderer>();
            this.renderer.material = new Material(this.decalMaterial);
            this.counter = num = this.counter + 1;
            this.renderer.material.renderQueue = this.decalMaterial.renderQueue + num;
            this.renderer.shadowCastingMode = ShadowCastingMode.Off;
            this.renderer.receiveShadows = true;
            this.renderer.useLightProbes = true;
            obj2.transform.position = Vector3.zero;
            obj2.transform.rotation = Quaternion.identity;
        }

        public void Start()
        {
            this.CreateMesh();
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                this.CreateMesh();
                this.UpdateDecalMesh();
            }
            if (!this.updateEveryFrame)
            {
                this.UpdateDecalMesh();
            }
        }

        private void UpdateDecalMesh()
        {
            DecalProjection decalProjection = new DecalProjection {
                Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)),
                Distantion = this.projectDistantion,
                HalfSize = this.projectSize,
                AtlasHTilesCount = this.hTilesCount,
                AtlasVTilesCount = this.vTilesCount,
                SurfaceAtlasPositions = this.surfaceAtlasPositions
            };
            this.meshBuilder.Build(decalProjection, ref this.decalMesh);
        }
    }
}

