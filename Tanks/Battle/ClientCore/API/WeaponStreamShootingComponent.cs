namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    [Shared, SerialVersionUID(0x5e6bfafbcd7097d5L)]
    public class WeaponStreamShootingComponent : TimeValidateComponent
    {
        public WeaponStreamShootingComponent()
        {
        }

        public WeaponStreamShootingComponent(Date startShootingTime)
        {
            this.StartShootingTime = startShootingTime;
        }

        [ProtocolOptional]
        public Date StartShootingTime { get; set; }
    }
}

