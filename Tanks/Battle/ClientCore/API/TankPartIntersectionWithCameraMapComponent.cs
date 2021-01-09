namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TankPartIntersectionWithCameraMapComponent : Component
    {
        public TankPartIntersectionWithCameraMapComponent(TankPartIntersectionWithCameraData[] tankPartIntersectionMap)
        {
            this.TankPartIntersectionMap = tankPartIntersectionMap;
        }

        public TankPartIntersectionWithCameraData[] TankPartIntersectionMap { get; set; }
    }
}

