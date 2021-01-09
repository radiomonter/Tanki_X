namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e1615d4a9aL)]
    public class FlagInstanceComponent : Component
    {
        public GameObject FlagInstance { get; set; }

        public GameObject FlagBeam { get; set; }
    }
}

