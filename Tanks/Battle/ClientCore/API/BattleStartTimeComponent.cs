namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14e775e2fa4L)]
    public class BattleStartTimeComponent : Component
    {
        [ProtocolOptional]
        public Date RoundStartTime { get; set; }
    }
}

