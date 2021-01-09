namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-2440064891528955383L)]
    public class TeamScoreComponent : Component
    {
        public int Score { get; set; }
    }
}

