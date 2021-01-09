namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14d6585ead1L)]
    public class MineConfigComponent : Component
    {
        public float DamageFrom { get; set; }

        public float DamageTo { get; set; }

        public long ActivationTime { get; set; }

        public float Impact { get; set; }

        public float BeginHideDistance { get; set; }

        public float HideRange { get; set; }
    }
}

