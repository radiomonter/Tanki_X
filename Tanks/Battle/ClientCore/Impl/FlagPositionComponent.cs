namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(-7424433796811681217L)]
    public class FlagPositionComponent : Component
    {
        public Vector3 Position { get; set; }
    }
}

