namespace Edelweiss.DecalSystem
{
    using System;

    public abstract class GenericDecalProjector<D, P, DM> : GenericDecalProjectorBase where D: GenericDecals<D, P, DM> where P: GenericDecalProjector<D, P, DM> where DM: GenericDecalsMesh<D, P, DM>
    {
        private DM m_DecalsMesh;

        protected GenericDecalProjector()
        {
        }

        internal void ResetDecalsMesh()
        {
            this.DecalsMesh = null;
            base.IsActiveProjector = false;
            base.DecalsMeshLowerVertexIndex = 0;
            base.DecalsMeshUpperVertexIndex = 0;
            base.IsUV1ProjectionCalculated = false;
            base.IsUV2ProjectionCalculated = false;
            base.IsTangentProjectionCalculated = false;
        }

        public DM DecalsMesh
        {
            get => 
                this.m_DecalsMesh;
            internal set => 
                this.m_DecalsMesh = value;
        }
    }
}

