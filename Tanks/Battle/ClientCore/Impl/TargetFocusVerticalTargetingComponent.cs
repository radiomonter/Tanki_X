namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4ba3e24e27b3bL)]
    public class TargetFocusVerticalTargetingComponent : Component
    {
        public float AdditionalAngleUp { get; set; }

        public float AdditionalAngleDown { get; set; }
    }
}

