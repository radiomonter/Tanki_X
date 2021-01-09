namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-2203330189936241204L)]
    public class RemoteSplashHitEvent : RemoteHitEvent
    {
        [ProtocolOptional]
        public List<HitTarget> SplashTargets { get; set; }
    }
}

