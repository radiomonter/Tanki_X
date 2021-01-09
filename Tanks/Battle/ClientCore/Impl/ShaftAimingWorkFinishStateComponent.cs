namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-5670596162316552032L)]
    public class ShaftAimingWorkFinishStateComponent : TimeValidateComponent
    {
        public ShaftAimingWorkFinishStateComponent()
        {
            this.FinishTimer = 0f;
        }

        public float FinishTimer { get; set; }
    }
}

