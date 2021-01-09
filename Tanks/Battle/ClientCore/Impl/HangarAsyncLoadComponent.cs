namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class HangarAsyncLoadComponent : Component
    {
        public UnityEngine.AsyncOperation AsyncOperation { get; set; }
    }
}

