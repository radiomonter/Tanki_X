namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e0d51ac44aL)]
    public class ResourceDataComponent : Component
    {
        public string Name { get; set; }

        public Object Data { get; set; }
    }
}

