namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x159dfd7e58dL)]
    public class UnitMoveEvent : Event
    {
        public UnitMoveEvent()
        {
        }

        public UnitMoveEvent(Movement move)
        {
            this.UnitMove = move;
        }

        public Movement UnitMove { get; set; }
    }
}

