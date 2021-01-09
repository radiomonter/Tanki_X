namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PauseMessageComponent : Component
    {
        public string Message { get; set; }
    }
}

