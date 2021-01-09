namespace Tanks.Battle.ClientCore
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class HammerTargetSectorCollectorSystem : ECSSystem
    {
        [OnEventFire]
        public void CollectTargetSectors(CollectTargetSectorsEvent evt, HammerNode weaponNode)
        {
            HammerPelletConeComponent hammerPelletCone = weaponNode.hammerPelletCone;
            evt.HAllowableAngleAcatter = hammerPelletCone.HorizontalConeHalfAngle;
            evt.VAllowableAngleAcatter = hammerPelletCone.VerticalConeHalfAngle;
        }

        public class HammerNode : Node
        {
            public HammerComponent hammer;
            public HammerPelletConeComponent hammerPelletCone;
        }
    }
}

