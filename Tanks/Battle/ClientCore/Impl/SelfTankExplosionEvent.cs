namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151157fea22L)]
    public class SelfTankExplosionEvent : Event
    {
        public bool CanDetachWeapon { get; set; }
    }
}

