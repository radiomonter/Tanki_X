namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HammerHitSystem : ECSSystem
    {
        [OnEventComplete]
        public void SendShot(ShotPrepareEvent evt, NotUnblockedHammerNode hammer)
        {
            SelfHammerShotEvent eventInstance = new SelfHammerShotEvent {
                RandomSeed = hammer.hammerPelletCone.ShotSeed
            };
            eventInstance.ShotDirection = new MuzzleLogicAccessor(hammer.muzzlePoint, hammer.weaponInstance).GetFireDirectionWorld();
            base.ScheduleEvent(eventInstance, hammer);
        }

        [OnEventComplete]
        public void SendShot(SendShotToServerEvent evt, UnblockedHammerNode hammer)
        {
            SelfHammerShotEvent eventInstance = new SelfHammerShotEvent {
                RandomSeed = hammer.hammerPelletCone.ShotSeed,
                ShotDirection = evt.TargetingData.BestDirection.Dir
            };
            base.ScheduleEvent(eventInstance, hammer);
        }

        [OnEventFire]
        public void SetShotFrame(NodeAddedEvent e, HammerNode hammer)
        {
            hammer.hammerPelletCone.ShotSeed = Time.frameCount;
        }

        [OnEventComplete]
        public void SetShotFrame(SelfHammerShotEvent e, HammerNode hammer)
        {
            hammer.hammerPelletCone.ShotSeed = Time.frameCount;
        }

        public class HammerNode : Node
        {
            public HammerComponent hammer;
            public HammerPelletConeComponent hammerPelletCone;
            public MuzzlePointComponent muzzlePoint;
        }

        [Not(typeof(WeaponUnblockedComponent))]
        public class NotUnblockedHammerNode : HammerHitSystem.HammerNode
        {
            public WeaponInstanceComponent weaponInstance;
        }

        public class UnblockedHammerNode : HammerHitSystem.HammerNode
        {
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

