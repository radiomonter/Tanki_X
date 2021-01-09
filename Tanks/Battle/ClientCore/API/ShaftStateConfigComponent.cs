namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d3593a470d66eeL), Shared]
    public class ShaftStateConfigComponent : Component
    {
        public float WaitingToActivationTransitionTimeSec { get; set; }

        public float ActivationToWorkingTransitionTimeSec { get; set; }

        public float FinishToIdleTransitionTimeSec { get; set; }
    }
}

