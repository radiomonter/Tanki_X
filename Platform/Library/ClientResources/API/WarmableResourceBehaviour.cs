namespace Platform.Library.ClientResources.API
{
    using System;
    using UnityEngine;

    public abstract class WarmableResourceBehaviour : MonoBehaviour
    {
        protected WarmableResourceBehaviour()
        {
        }

        public abstract void WarmUp();
    }
}

