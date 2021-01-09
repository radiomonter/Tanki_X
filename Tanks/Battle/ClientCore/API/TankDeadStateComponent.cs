namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    [Shared, SerialVersionUID(-2656312914607478436L)]
    public class TankDeadStateComponent : Component
    {
        private Date endDate;

        public Date EndDate
        {
            get => 
                this.endDate;
            set => 
                this.endDate = value;
        }
    }
}

