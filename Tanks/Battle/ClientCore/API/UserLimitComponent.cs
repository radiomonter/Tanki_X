namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x364816cb5d9af3b5L)]
    public class UserLimitComponent : Component
    {
        public int UserLimit { get; set; }

        public int TeamLimit { get; set; }
    }
}

