namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class IsisEvaluateTeamTankEvent : Event
    {
        public IsisEvaluateTeamTankEvent()
        {
        }

        public IsisEvaluateTeamTankEvent(Tanks.Battle.ClientCore.API.TargetData targetData, long shooterTeamKey)
        {
            this.TargetData = targetData;
            this.ShooterTeamKey = shooterTeamKey;
        }

        public Tanks.Battle.ClientCore.API.TargetData TargetData { get; set; }

        public long ShooterTeamKey { get; set; }
    }
}

