namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MapLoadPreviewComponent : Component
    {
        public string AssetGuid { get; set; }
    }
}

