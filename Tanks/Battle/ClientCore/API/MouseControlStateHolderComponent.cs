namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MouseControlStateHolderComponent : Component
    {
        public bool MouseControlAllowed { get; set; }

        public bool MouseVerticalInverted { get; set; }

        public bool MouseControlEnable { get; set; }

        public float MouseSensivity { get; set; }
    }
}

