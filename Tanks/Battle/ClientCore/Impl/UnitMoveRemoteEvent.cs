namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x159fea606efL), Shared]
    public class UnitMoveRemoteEvent : UnitMoveEvent
    {
        public UnitMoveRemoteEvent()
        {
        }

        public UnitMoveRemoteEvent(Movement move) : base(move)
        {
        }
    }
}

