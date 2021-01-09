namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class VerticalSectorsTargetingComponent : Component
    {
        public float WorkDistance { get; set; }

        public float VAngleUp { get; set; }

        public float VAngleDown { get; set; }

        public float HAngle { get; set; }

        public float RaysPerDegree { get; set; }
    }
}

