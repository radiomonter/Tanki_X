namespace Edelweiss.DecalSystem
{
    using Edelweiss.TextureAtlas;
    using System;
    using UnityEngine;

    public abstract class GenericDecalsBase : MonoBehaviour
    {
        private Transform m_CachedTransform;
        public const int c_MaximumVertexCount = 0xffff;
        private GenericDecalsMeshBase m_LinkedDecalsMesh;
        [SerializeField]
        private ProjectionMode m_ProjectionMode;
        [SerializeField]
        private UVMode m_UVMode;
        [SerializeField]
        private UV2Mode m_UV2Mode;
        [SerializeField]
        private NormalsMode m_NormalsMode = NormalsMode.Target;
        [SerializeField]
        private TangentsMode m_TangentsMode;
        [SerializeField]
        private bool m_UseVertexColors;
        [SerializeField]
        private Color m_VertexColorTint = Color.white;
        [SerializeField]
        private bool m_AffectSameLODOnly;
        [SerializeField]
        private Edelweiss.DecalSystem.LightmapUpdateMode m_LightmapUpdateMode;
        [SerializeField]
        private bool m_AreRenderersEditable;
        [SerializeField]
        private TextureAtlasType m_TextureAtlasType;
        [SerializeField]
        private TextureAtlasAsset m_TextureAtlasAsset;
        [SerializeField]
        private Material m_Material;
        public UVRectangle[] uvRectangles = new UVRectangle[0];
        public UVRectangle[] uv2Rectangles = new UVRectangle[0];
        [SerializeField]
        private bool m_AreDecalsMeshesOptimized;
        [SerializeField]
        private string m_MeshAssetFolder = "Assets";

        protected GenericDecalsBase()
        {
        }

        public virtual void ApplyMaterialToMeshRenderers()
        {
            if (Edition.IsDecalSystemFree && (this.CurrentTextureAtlasType == TextureAtlasType.Reference))
            {
                throw new InvalidOperationException("Texture atlas assets are only supported in Decal System Pro.");
            }
        }

        public virtual void ApplyRenderersEditability()
        {
            if (Edition.IsDecalSystemFree && this.m_AreRenderersEditable)
            {
                this.m_AreRenderersEditable = false;
            }
        }

        public abstract void InitializeDecalsMeshRenderers();
        public abstract void OnEnable();
        public virtual void OptimizeDecalsMeshes()
        {
            this.m_AreDecalsMeshesOptimized = true;
        }

        public void SetDecalsMeshesAreNotOptimized()
        {
            this.m_AreDecalsMeshesOptimized = false;
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

        public GenericDecalsMeshBase LinkedDecalsMesh
        {
            get => 
                this.m_LinkedDecalsMesh;
            internal set => 
                this.m_LinkedDecalsMesh = value;
        }

        public bool IsLinkedWithADecalsMesh =>
            !ReferenceEquals(this.LinkedDecalsMesh, null);

        public abstract bool CastShadows { get; set; }

        public abstract bool ReceiveShadows { get; set; }

        public abstract bool UseLightProbes { get; set; }

        public abstract Transform LightProbeAnchor { get; set; }

        public ProjectionMode CurrentProjectionMode
        {
            get => 
                this.m_ProjectionMode;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The projection mode can't be changed as long as the instance is linked with a decals mesh.");
                }
                this.m_ProjectionMode = value;
                if (this.m_ProjectionMode == ProjectionMode.Diffuse)
                {
                    this.m_UVMode = UVMode.Project;
                    this.m_UV2Mode = UV2Mode.None;
                    this.m_NormalsMode = NormalsMode.Target;
                    this.m_TangentsMode = TangentsMode.None;
                }
                else if (this.m_ProjectionMode == ProjectionMode.BumpedDiffuse)
                {
                    this.m_UVMode = UVMode.Project;
                    this.m_UV2Mode = UV2Mode.None;
                    this.m_NormalsMode = NormalsMode.Target;
                    this.m_TangentsMode = TangentsMode.Project;
                }
                else if (this.m_ProjectionMode == ProjectionMode.LightmappedDiffuse)
                {
                    this.m_UVMode = UVMode.Project;
                    this.m_UV2Mode = UV2Mode.Lightmapping;
                    this.m_NormalsMode = NormalsMode.Target;
                    this.m_TangentsMode = TangentsMode.None;
                }
                else if (this.m_ProjectionMode == ProjectionMode.LightmappedBumpedDiffuse)
                {
                    this.m_UVMode = UVMode.Project;
                    this.m_UV2Mode = UV2Mode.Lightmapping;
                    this.m_NormalsMode = NormalsMode.Target;
                    this.m_TangentsMode = TangentsMode.Project;
                }
                else if (this.m_ProjectionMode == ProjectionMode.BumpOfTarget)
                {
                    this.m_UVMode = UVMode.Project;
                    this.m_UV2Mode = UV2Mode.TargetUV;
                    this.m_NormalsMode = NormalsMode.Target;
                    this.m_TangentsMode = TangentsMode.Target;
                }
            }
        }

        public UVMode CurrentUVMode
        {
            get => 
                this.m_UVMode;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The uv mode can't be changed as long as the instance is linked with a decals mesh.");
                }
                if (this.CurrentProjectionMode != ProjectionMode.Advanced)
                {
                    throw new InvalidOperationException("Setting a new uv mode is only possible in the advanced projection mode!");
                }
                this.m_UVMode = value;
            }
        }

        public UV2Mode CurrentUV2Mode
        {
            get => 
                this.m_UV2Mode;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The uv2 mode can't be changed as long as the instance is linked with a decals mesh.");
                }
                if (this.CurrentProjectionMode != ProjectionMode.Advanced)
                {
                    throw new InvalidOperationException("Setting a new uv2 mode is only possible in the advanced projection mode!");
                }
                this.m_UV2Mode = value;
            }
        }

        public NormalsMode CurrentNormalsMode
        {
            get => 
                this.m_NormalsMode;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The normals mode can't be changed as long as the instance is linked with a decals mesh.");
                }
                if (this.CurrentProjectionMode != ProjectionMode.Advanced)
                {
                    throw new InvalidOperationException("Setting a new normals mode is only possible in the advanced projection mode!");
                }
                this.m_NormalsMode = value;
            }
        }

        public TangentsMode CurrentTangentsMode
        {
            get => 
                this.m_TangentsMode;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The tangents mode can't be changed as long as the instance is linked with a decals mesh.");
                }
                if (this.CurrentProjectionMode != ProjectionMode.Advanced)
                {
                    throw new InvalidOperationException("Setting a new tangents mode is only possible in the advanced projection mode!");
                }
                this.m_TangentsMode = value;
            }
        }

        public bool UseVertexColors
        {
            get => 
                this.m_UseVertexColors;
            set
            {
                if (this.IsLinkedWithADecalsMesh)
                {
                    throw new InvalidOperationException("The vertex color mode can't be changed as long as the instance is linked with a decals mesh.'");
                }
                if (value && Edition.IsDecalSystemFree)
                {
                    throw new InvalidOperationException("Vertex colors can only be used in Decal System Pro.");
                }
                this.m_UseVertexColors = value;
            }
        }

        public Color VertexColorTint
        {
            get => 
                this.m_VertexColorTint;
            set => 
                this.m_VertexColorTint = value;
        }

        public bool AffectSameLODOnly
        {
            get => 
                this.m_AffectSameLODOnly;
            set
            {
                if (Application.isPlaying)
                {
                    throw new InvalidOperationException("This operation can only be executed in the editor, meaning while the application is not playing.");
                }
                this.m_AffectSameLODOnly = value;
            }
        }

        public Edelweiss.DecalSystem.LightmapUpdateMode LightmapUpdateMode
        {
            get => 
                this.m_LightmapUpdateMode;
            set
            {
                if (Application.isPlaying)
                {
                    throw new InvalidOperationException("This operation can only be executed in the editor, meaning while the application is not playing.");
                }
                this.m_LightmapUpdateMode = value;
            }
        }

        public bool AreRenderersEditable
        {
            get => 
                this.m_AreRenderersEditable;
            set
            {
                if (Edition.IsDecalSystemFree && value)
                {
                    throw new InvalidOperationException("The renderer editability can only be used in Decal System Pro.");
                }
                this.m_AreRenderersEditable = value;
                this.ApplyRenderersEditability();
            }
        }

        public TextureAtlasType CurrentTextureAtlasType
        {
            get => 
                this.m_TextureAtlasType;
            set
            {
                if (Edition.IsDecalSystemFree && (value == TextureAtlasType.Reference))
                {
                    throw new InvalidOperationException("Texture atlas assets can only be used in Decal System Pro.");
                }
                this.m_TextureAtlasType = value;
                this.ApplyMaterialToMeshRenderers();
            }
        }

        public TextureAtlasAsset CurrentTextureAtlasAsset
        {
            get => 
                this.m_TextureAtlasAsset;
            set
            {
                this.m_TextureAtlasAsset = value;
                if (!Edition.IsDecalSystemFree && (this.CurrentTextureAtlasType == TextureAtlasType.Reference))
                {
                    this.ApplyMaterialToMeshRenderers();
                }
            }
        }

        public Material CurrentMaterial
        {
            get => 
                this.m_Material;
            set
            {
                this.m_Material = value;
                if (this.CurrentTextureAtlasType == TextureAtlasType.Builtin)
                {
                    this.ApplyMaterialToMeshRenderers();
                }
            }
        }

        public UVRectangle[] CurrentUvRectangles
        {
            get
            {
                UVRectangle[] uvRectangles = null;
                if (this.CurrentTextureAtlasType == TextureAtlasType.Reference)
                {
                    if (this.CurrentTextureAtlasAsset != null)
                    {
                        uvRectangles = this.CurrentTextureAtlasAsset.uvRectangles;
                    }
                }
                else if (this.CurrentTextureAtlasType == TextureAtlasType.Builtin)
                {
                    uvRectangles = this.uvRectangles;
                }
                return uvRectangles;
            }
        }

        public UVRectangle[] CurrentUv2Rectangles
        {
            get
            {
                UVRectangle[] rectangleArray = null;
                if (this.CurrentTextureAtlasType == TextureAtlasType.Reference)
                {
                    if (this.CurrentTextureAtlasAsset != null)
                    {
                        rectangleArray = this.CurrentTextureAtlasAsset.uv2Rectangles;
                    }
                }
                else if (this.CurrentTextureAtlasType == TextureAtlasType.Builtin)
                {
                    rectangleArray = this.uv2Rectangles;
                }
                return rectangleArray;
            }
        }

        public bool AreDecalsMeshesOptimized =>
            this.m_AreDecalsMeshesOptimized;

        public string MeshAssetFolder
        {
            get => 
                this.m_MeshAssetFolder;
            set => 
                this.m_MeshAssetFolder = value;
        }
    }
}

