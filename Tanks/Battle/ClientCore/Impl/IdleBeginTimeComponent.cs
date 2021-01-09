namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class IdleBeginTimeComponent : Component
    {
        private Date? idleBeginTime;

        public Date? IdleBeginTime
        {
            get => 
                this.idleBeginTime;
            set => 
                this.idleBeginTime = value;
        }
    }
}

