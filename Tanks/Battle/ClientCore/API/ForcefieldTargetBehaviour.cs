namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ForcefieldTargetBehaviour : TargetBehaviour
    {
        public bool OwnerTeamCanShootThrough;

        public override bool AcceptAsTarget(Entity targetingOwner) => 
            false;

        public override bool CanSkip(Entity targetingOwner) => 
            this.CheckCanSkip(targetingOwner);

        private bool CheckCanSkip(Entity targetingOwner) => 
            this.OwnerTeamCanShootThrough && (base.TargetEntity.IsSameGroup<TankGroupComponent>(targetingOwner) || base.TargetEntity.IsSameGroup<TeamGroupComponent>(targetingOwner));
    }
}

