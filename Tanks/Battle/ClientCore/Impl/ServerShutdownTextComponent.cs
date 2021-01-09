namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d37d8e870e1c7aL)]
    public class ServerShutdownTextComponent : Component
    {
        public string Text { get; set; }
    }
}

