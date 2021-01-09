namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShotToHitSystem : ECSSystem
    {
        [OnEventFire]
        public void GenerateShotId(BeforeShotEvent e, SingleNode<ShotIdComponent> shotId)
        {
            shotId.component.NextShotId();
        }

        [OnEventFire]
        public void SetShotIdToBaseHitEvent(HitEvent e, NotBulletWeaponNode weapon, [JoinByTank] SingleNode<TankSyncComponent> selfTank)
        {
            e.ShotId = weapon.shotId.ShotId;
        }

        [OnEventFire]
        public void SetShotIdToBaseShotEvent(BaseShotEvent e, SingleNode<ShotIdComponent> shotId, [JoinByTank] SingleNode<TankSyncComponent> selfTank)
        {
            e.ShotId = shotId.component.ShotId;
        }

        [Not(typeof(RicochetComponent)), Not(typeof(TwinsComponent))]
        public class NotBulletWeaponNode : Node
        {
            public ShotIdComponent shotId;
        }
    }
}

