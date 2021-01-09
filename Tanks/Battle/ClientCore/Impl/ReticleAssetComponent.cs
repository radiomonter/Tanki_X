namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ReticleAssetComponent : Component
    {
        public ReticleAssetComponent()
        {
        }

        public ReticleAssetComponent(AssetReference reference)
        {
            this.Reference = reference;
        }

        public AssetReference Reference { get; set; }
    }
}

