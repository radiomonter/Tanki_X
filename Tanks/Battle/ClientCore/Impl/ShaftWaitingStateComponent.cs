namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x5ac8d473a246946aL)]
    public class ShaftWaitingStateComponent : TimeValidateComponent
    {
        public ShaftWaitingStateComponent()
        {
            this.WaitingTimer = 0f;
        }

        public float WaitingTimer { get; set; }
    }
}

