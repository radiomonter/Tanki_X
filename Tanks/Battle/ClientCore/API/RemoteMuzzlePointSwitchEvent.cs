namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-8312866616397669979L)]
    public class RemoteMuzzlePointSwitchEvent : Event
    {
        public int Index { get; set; }
    }
}

