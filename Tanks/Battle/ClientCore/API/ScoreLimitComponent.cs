namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-3048295118496552479L)]
    public class ScoreLimitComponent : Component
    {
        public int ScoreLimit { get; set; }
    }
}

