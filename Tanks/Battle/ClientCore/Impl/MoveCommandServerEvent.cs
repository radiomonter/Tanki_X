namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-4956413533647444536L)]
    public class MoveCommandServerEvent : Event
    {
        public Tanks.Battle.ClientCore.Impl.MoveCommand MoveCommand { get; set; }
    }
}

