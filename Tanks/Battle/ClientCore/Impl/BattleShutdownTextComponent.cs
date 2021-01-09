namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d379bfb9b11cd6L)]
    public class BattleShutdownTextComponent : Component
    {
        public string Text { get; set; }
    }
}

