namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HangarAssetComponent : Component
    {
        public string AssetGuid { get; set; }
    }
}

