namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x5d14af7078b88080L)]
    public class DamageWeakeningByTargetComponent : Component
    {
        public DamageWeakeningByTargetComponent()
        {
        }

        public DamageWeakeningByTargetComponent(float damagePercent)
        {
            this.DamagePercent = damagePercent;
        }

        public float DamagePercent { get; set; }
    }
}

