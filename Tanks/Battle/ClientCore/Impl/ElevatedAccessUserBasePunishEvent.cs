namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ElevatedAccessUserBasePunishEvent : Event
    {
        public string Uid { get; set; }

        public string Reason { get; set; }
    }
}

