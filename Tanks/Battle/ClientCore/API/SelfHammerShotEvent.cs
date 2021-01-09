namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-1937089974629265090L)]
    public class SelfHammerShotEvent : SelfShotEvent
    {
        public int RandomSeed { get; set; }
    }
}

