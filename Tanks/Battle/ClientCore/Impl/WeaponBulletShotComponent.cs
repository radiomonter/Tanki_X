namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ed8954753L)]
    public class WeaponBulletShotComponent : Component
    {
        public float BulletRadius { get; set; }

        public float BulletSpeed { get; set; }
    }
}

