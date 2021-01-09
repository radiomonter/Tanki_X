namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x43fa516698a74fdeL)]
    public class FlagPedestalComponent : Component
    {
        public Vector3 Position { get; set; }
    }
}

