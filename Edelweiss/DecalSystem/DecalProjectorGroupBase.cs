namespace Edelweiss.DecalSystem
{
    using System;
    using UnityEngine;

    public abstract class DecalProjectorGroupBase : MonoBehaviour
    {
        protected DecalProjectorGroupBase()
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
    }
}

