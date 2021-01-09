namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x15929a652ebL), Shared]
    public class DroneWeaponComponent : Component
    {
        public float lastTimeTargetSeen;
        public float lastControlTime;
    }
}

