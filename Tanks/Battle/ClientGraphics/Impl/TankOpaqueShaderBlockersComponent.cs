namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TankOpaqueShaderBlockersComponent : Component
    {
        public TankOpaqueShaderBlockersComponent()
        {
            this.Blockers = new HashSet<string>();
        }

        public HashSet<string> Blockers { get; set; }
    }
}

