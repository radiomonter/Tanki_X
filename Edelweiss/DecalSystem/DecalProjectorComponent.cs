namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public class DecalProjectorComponent : GenericDecalProjectorComponent<Decals, DecalProjectorBase, DecalsMesh>
    {
        public bool ignoreMeshMinimizer;
        public bool useCustomMeshMinimizerMaximumErrors;
        [SerializeField]
        private float m_MeshMinimizerMaximumAbsoluteError = 0.0001f;
        [SerializeField]
        private float m_MeshMinimizerMaximumRelativeError = 0.0001f;
        public bool affectMeshes = true;
        public bool affectTerrains = true;
        public bool useTerrainDensity;
        public float terrainDensity = 0.5f;

        public float MeshMinimizerMaximumAbsoluteError
        {
            get => 
                this.m_MeshMinimizerMaximumAbsoluteError;
            set
            {
                if (value < 0f)
                {
                    throw new ArgumentOutOfRangeException("The maximum absolute error has to be greater than zero.");
                }
                this.m_MeshMinimizerMaximumAbsoluteError = value;
            }
        }

        public float MeshMinimizerMaximumRelativeError
        {
            get => 
                this.m_MeshMinimizerMaximumRelativeError;
            set
            {
                if ((value < 0f) || (value > 1f))
                {
                    throw new ArgumentOutOfRangeException("The maximum relative error has to be within [0.0f, 1.0f].");
                }
                this.m_MeshMinimizerMaximumRelativeError = value;
            }
        }
    }
}

