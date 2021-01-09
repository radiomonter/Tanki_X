namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HolyshiedTargetBehaviour : TargetBehaviour
    {
        public override bool AcceptAsTarget(Entity targetingOwner) => 
            false;

        public override bool CanSkip(Entity targetingOwner) => 
            base.TargetEntity.IsSameGroup<TeamGroupComponent>(targetingOwner);
    }
}

