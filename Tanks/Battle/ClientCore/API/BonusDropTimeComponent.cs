namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-7944772313373733709L)]
    public class BonusDropTimeComponent : Component
    {
        public BonusDropTimeComponent()
        {
        }

        public BonusDropTimeComponent(Date dropTime)
        {
            this.DropTime = dropTime;
        }

        public Date DropTime { get; set; }
    }
}

