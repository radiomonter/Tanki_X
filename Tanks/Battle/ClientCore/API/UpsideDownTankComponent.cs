namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UpsideDownTankComponent : Component
    {
        public Date TimeTankBecomesUpsideDown { get; set; }

        public bool Removed { get; set; }
    }
}

