namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    [Shared, SerialVersionUID(0x480fcab1fd9cf293L)]
    public class DurationComponent : Component
    {
        private Date startedTime;

        public Date StartedTime
        {
            get => 
                this.startedTime;
            set => 
                this.startedTime = value;
        }
    }
}

