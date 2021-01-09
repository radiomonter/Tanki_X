namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AttachResourceEvent : Event
    {
        public Object Data { get; set; }

        public string Name { get; set; }
    }
}

