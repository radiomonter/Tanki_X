namespace Edelweiss.DecalSystem
{
    using System;

    public abstract class SkinnedDecalProjectorBase : GenericDecalProjector<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
    {
        private int m_DecalsMeshLowerBonesIndex;
        private int m_DecalsMeshUpperBonesIndex;

        protected SkinnedDecalProjectorBase()
        {
        }

        public int DecalsMeshLowerBonesIndex
        {
            get => 
                this.m_DecalsMeshLowerBonesIndex;
            internal set => 
                this.m_DecalsMeshLowerBonesIndex = value;
        }

        public int DecalsMeshUpperBonesIndex
        {
            get => 
                this.m_DecalsMeshUpperBonesIndex;
            internal set => 
                this.m_DecalsMeshUpperBonesIndex = value;
        }

        public int DecalsMeshBonesCount =>
            (this.DecalsMeshUpperBonesIndex - this.DecalsMeshLowerBonesIndex) + 1;
    }
}

