namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x159fea5dd81L), Shared]
    public class UnitMoveSelfEvent : UnitMoveEvent
    {
        public UnitMoveSelfEvent()
        {
        }

        public UnitMoveSelfEvent(Movement move) : base(move)
        {
        }
    }
}

