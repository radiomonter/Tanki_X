namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AttachToScreenComponent : MonoBehaviour, Component
    {
        public EntityBehaviour JoinEntityBehaviour { get; set; }
    }
}

