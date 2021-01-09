namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x1604fd2b232L)]
    public class PersonalXCrystalBonusComponent : Component, ComponentServerChangeListener
    {
        private bool finished = true;

        public void ChangedOnServer(Entity entity)
        {
            if (this.Finished != this.finished)
            {
                this.finished = this.Finished;
                if (this.finished)
                {
                    EngineService.Engine.ScheduleEvent<FinishPersonalXCrystalBonusEvent>(entity);
                }
                else
                {
                    EngineService.Engine.ScheduleEvent<StartPersonalXCrystalBonusEvent>(entity);
                }
            }
        }

        public long timeToEndSec()
        {
            float num = (float) (this.EndDate - Date.Now);
            Debug.Log("PersonalXCrystalBonusComponent.timeToEndSec delta=" + num);
            return (long) (num / 1000f);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public long StartTime { get; set; }

        public long DurationInSec { get; set; }

        public Date EndDate { get; set; }

        public double Multiplier { get; set; }

        public bool Used { get; set; }

        public bool Finished { get; set; }
    }
}

