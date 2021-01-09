namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x77ca0575647c0accL)]
    public class ShaftAimingWorkActivationStateComponent : TimeValidateComponent
    {
        public ShaftAimingWorkActivationStateComponent()
        {
            this.ActivationTimer = 0f;
        }

        public float ActivationTimer { get; set; }
    }
}

