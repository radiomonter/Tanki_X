﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class VulcanBandAnimationSystem : ECSSystem
    {
        [OnEventFire]
        public void InitBandAnimation(NodeAddedEvent evt, InitialVulcanBandAnimationNode weapon)
        {
            weapon.vulcanBandAnimation.InitBand(weapon.baseRenderer.Renderer, weapon.Entity);
            weapon.Entity.AddComponent<VulcanBandAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartBandAnimation(NodeAddedEvent evt, ReadyVulcanBandAnimationShootingNode weapon)
        {
            weapon.vulcanBandAnimation.StartBandAnimation();
        }

        [OnEventFire]
        public void StopBandAnimation(NodeRemoveEvent evt, ReadyVulcanBandAnimationShootingNode weapon)
        {
            weapon.vulcanBandAnimation.StopBandAnimation();
        }

        [OnEventFire]
        public void StopBandAnimationOnDeath(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyVulcanBandAnimationShootingNode weapon)
        {
            weapon.vulcanBandAnimation.StopBandAnimation();
        }

        public class ActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
        }

        public class InitialVulcanBandAnimationNode : Node
        {
            public VulcanBandAnimationComponent vulcanBandAnimation;
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
        }

        public class ReadyVulcanBandAnimationShootingNode : Node
        {
            public WeaponStreamShootingComponent weaponStreamShooting;
            public VulcanBandAnimationComponent vulcanBandAnimation;
            public VulcanBandAnimationReadyComponent vulcanBandAnimationReady;
            public TankGroupComponent tankGroup;
        }
    }
}

