namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class MuzzlePointSwitchSystem : ECSSystem
    {
        [OnEventComplete]
        public void RemoteSwitchMuzzlePoint(RemoteMuzzlePointSwitchEvent e, WeaponMultyMuzzleNode weaponNode)
        {
            MuzzlePointComponent muzzlePoint = weaponNode.muzzlePoint;
            muzzlePoint.CurrentIndex = e.Index % muzzlePoint.Points.Length;
        }

        [OnEventFire]
        public void SelfSwitchMuzzlePoint(PostShotEvent e, WeaponMultyMuzzleNode weaponNode)
        {
            MuzzlePointComponent muzzlePoint = weaponNode.muzzlePoint;
            muzzlePoint.CurrentIndex = (muzzlePoint.CurrentIndex + 1) % muzzlePoint.Points.Length;
            base.ScheduleEvent(new MuzzlePointSwitchEvent(muzzlePoint.CurrentIndex), weaponNode);
        }

        public class WeaponMultyMuzzleNode : Node
        {
            public TwinsComponent twins;
            public MuzzlePointComponent muzzlePoint;
        }
    }
}

