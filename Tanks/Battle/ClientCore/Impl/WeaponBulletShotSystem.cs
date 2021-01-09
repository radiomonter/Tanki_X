namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class WeaponBulletShotSystem : ECSSystem
    {
        [OnEventComplete]
        public void PrepareBestDirection(ShotPrepareEvent evt, UnblockedWeaponNode weaponNode, [JoinByTank] SingleNode<TankSyncComponent> tankNode)
        {
            TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
            base.ScheduleEvent(new TargetingEvent(targetingData), weaponNode);
            base.ScheduleEvent(new SendShotToServerEvent(targetingData), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(BaseShotEvent evt, BlockedWeaponNode weaponNode)
        {
            MuzzleVisualAccessor accessor = new MuzzleVisualAccessor(weaponNode.muzzlePoint);
            base.ScheduleEvent(new BulletBuildEvent(accessor.GetFireDirectionWorld()), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(RemoteShotEvent evt, UnblockedWeaponNode weaponNode)
        {
            base.ScheduleEvent(new BulletBuildEvent(evt.ShotDirection), weaponNode);
        }

        [OnEventComplete]
        public void RequestBulletBuild(SendShotToServerEvent evt, UnblockedWeaponNode weaponNode)
        {
            base.ScheduleEvent(new BulletBuildEvent(evt.TargetingData.BestDirection.Dir), weaponNode);
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        public class BlockedWeaponNode : Node
        {
            public WeaponBulletShotComponent weaponBulletShot;
            public MuzzlePointComponent muzzlePoint;
            public WeaponBlockedComponent weaponBlocked;
        }

        public class UnblockedWeaponNode : Node
        {
            public WeaponBulletShotComponent weaponBulletShot;
            public MuzzlePointComponent muzzlePoint;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

