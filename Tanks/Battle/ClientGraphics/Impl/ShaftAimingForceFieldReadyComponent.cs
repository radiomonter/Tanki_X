namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShaftAimingForceFieldReadyComponent : Component
    {
        public ShaftAimingForceFieldReadyComponent(int propertyId)
        {
            this.PropertyID = propertyId;
        }

        public int PropertyID { get; set; }
    }
}

