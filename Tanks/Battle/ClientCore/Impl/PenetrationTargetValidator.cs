namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PenetrationTargetValidator : TargetValidator
    {
        public PenetrationTargetValidator(Entity ownerEntity) : base(ownerEntity)
        {
        }

        public override bool BreakOnTargetHit(Entity target) => 
            false;
    }
}

