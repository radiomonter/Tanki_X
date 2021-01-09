namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4ba3e0ba05a2dL)]
    public class TargetFocusConicTargetingComponent : Component
    {
        public float AdditionalHalfConeAngle { get; set; }
    }
}

