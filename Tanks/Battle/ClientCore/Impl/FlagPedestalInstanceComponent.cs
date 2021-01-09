namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e15e2e51caL)]
    public class FlagPedestalInstanceComponent : Component
    {
        public GameObject FlagPedestalInstance { get; set; }
    }
}

