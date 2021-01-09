namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x6093bf38eb872fdcL)]
    public class MoveCommandEvent : Event
    {
        public MoveCommandEvent()
        {
        }

        public MoveCommandEvent(Tanks.Battle.ClientCore.Impl.MoveCommand moveCommand)
        {
            this.MoveCommand = moveCommand;
        }

        public Tanks.Battle.ClientCore.Impl.MoveCommand MoveCommand { get; set; }
    }
}

