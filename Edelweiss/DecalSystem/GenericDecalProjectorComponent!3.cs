namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public abstract class GenericDecalProjectorComponent<D, P, DM> : GenericDecalProjectorBaseComponent where D: GenericDecals<D, P, DM> where P: GenericDecalProjector<D, P, DM> where DM: GenericDecalsMesh<D, P, DM>
    {
        protected GenericDecalProjectorComponent()
        {
        }

        public D GetDecals()
        {
            D component = null;
            for (Transform transform = base.CachedTransform; (transform != null) && (component == null); transform = transform.parent)
            {
                component = transform.GetComponent<D>();
            }
            return component;
        }
    }
}

