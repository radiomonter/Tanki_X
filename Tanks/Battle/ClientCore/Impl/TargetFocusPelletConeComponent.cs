namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4bc7696d7227fL)]
    public class TargetFocusPelletConeComponent : Component
    {
        public bool ChangePelletCount { get; set; }

        public float AdditionalHorizontalHalfConeAngle { get; set; }

        public float AdditionalVerticalHalfConeAngle { get; set; }
    }
}

