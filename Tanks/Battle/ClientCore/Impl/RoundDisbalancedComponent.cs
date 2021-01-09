namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x2a5a800bd35d6b02L)]
    public class RoundDisbalancedComponent : Component
    {
        public TeamColor Loser { get; set; }

        public int InitialDominationTimerSec { get; set; }

        public Date FinishTime { get; set; }
    }
}

