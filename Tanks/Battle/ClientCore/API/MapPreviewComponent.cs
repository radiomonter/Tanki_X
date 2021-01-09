namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MapPreviewComponent : Component
    {
        public string AssetGuid { get; set; }
    }
}

