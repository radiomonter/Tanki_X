namespace Platform.Library.ClientUnityIntegration.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DependAssetsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private List<Object> dependAssets;

        public List<Object> DependAssets
        {
            get => 
                this.dependAssets;
            set => 
                this.dependAssets = value;
        }
    }
}

