namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ResourceLoadStatComponent : Component
    {
        public ResourceLoadStatComponent()
        {
            this.BundleToProgress = new Dictionary<AssetBundleInfo, float>();
            this.BytesLoaded = 0;
            this.Progress = 0f;
        }

        public Dictionary<AssetBundleInfo, float> BundleToProgress { get; private set; }

        public int BytesTotal { get; set; }

        public int BytesLoaded { get; set; }

        public float Progress { get; set; }
    }
}

