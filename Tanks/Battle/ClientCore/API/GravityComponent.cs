namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x14e438eb0feL), Shared]
    public class GravityComponent : Component
    {
        public float Gravity { get; set; }

        public Tanks.Battle.ClientCore.API.GravityType GravityType { get; set; }
    }
}

