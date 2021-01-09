namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class WeaponShotSystem : ECSSystem
    {
        [OnEventFire]
        public void SendShot(ShotPrepareEvent evt, BlockedWeaponNode weaponNode)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            base.ScheduleEvent(new SelfShotEvent(accessor.GetFireDirectionWorld()), weaponNode);
        }

        [OnEventFire]
        public void SendShot(ShotPrepareEvent evt, UndergroundWeaponNode weaponNode)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            base.ScheduleEvent(new SelfShotEvent(accessor.GetFireDirectionWorld()), weaponNode);
        }

        [OnEventFire]
        public void SendShot(SendShotToServerEvent evt, UnblockedWeaponNode weaponNode)
        {
            SelfShotEvent eventInstance = new SelfShotEvent();
            if (evt.TargetingData.BestDirection != null)
            {
                eventInstance.ShotDirection = evt.TargetingData.BestDirection.Dir;
            }
            else
            {
                eventInstance.ShotDirection = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance).GetFireDirectionWorld();
            }
            base.ScheduleEvent(eventInstance, weaponNode);
        }

        public class BlockedWeaponNode : WeaponShotSystem.WeaponNode
        {
            public WeaponBlockedComponent weaponBlocked;
        }

        public class UnblockedWeaponNode : WeaponShotSystem.WeaponNode
        {
            public WeaponUnblockedComponent weaponUnblocked;
        }

        public class UndergroundWeaponNode : WeaponShotSystem.WeaponNode
        {
            public WeaponUndergroundComponent weaponUnderground;
        }

        public class WeaponNode : Node
        {
            public WeaponShotComponent weaponShot;
            public MuzzlePointComponent muzzlePoint;
            public ShotIdComponent shotId;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

