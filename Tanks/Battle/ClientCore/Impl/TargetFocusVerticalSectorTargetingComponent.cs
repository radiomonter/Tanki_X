namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4ba3e4778bf61L)]
    public class TargetFocusVerticalSectorTargetingComponent : Component
    {
        public float AdditionalAngleUp { get; set; }

        public float AdditionalAngleDown { get; set; }

        public float AdditionalAngleHorizontal { get; set; }
    }
}

