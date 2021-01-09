namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;

    public class ParticleSystemEffectsSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache2;
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache3;
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache4;
        [CompilerGenerated]
        private static Action<SkinParticleNode> <>f__am$cache5;

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = skin => skin.skinParticleSystemEffects.StopEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache4);
        }

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon, [Combine] TankParticleNode tankWithEffectNode)
        {
            tankWithEffectNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = skin => skin.skinParticleSystemEffects.StartEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache5);
        }

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon, [Combine] TankParticleNode tankWithEffectNode)
        {
            tankWithEffectNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankActive(NodeAddedEvent e, TankWithParticleActiveStateNode tankActiveStateNode)
        {
            tankActiveStateNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankActive(NodeAddedEvent e, TankActiveStateNode tankActiveStateNode, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = skin => skin.skinParticleSystemEffects.StartEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache0);
        }

        [OnEventFire]
        public void TankDead(NodeAddedEvent e, TankWithParticleDeadStateNode tankDeadStateNode)
        {
            tankDeadStateNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void TankDead(NodeAddedEvent e, TankDeadStateNode tankDeadStateNode, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = skin => skin.skinParticleSystemEffects.StopEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache1);
        }

        [OnEventFire]
        public void TankInvisible(ActivateTankInvisibilityEffectEvent e, TankParticleNode tankWithEffectNode)
        {
            tankWithEffectNode.particleSystemEffects.StopEmission();
        }

        [OnEventFire]
        public void TankInvisible(ActivateTankInvisibilityEffectEvent e, TankActiveStateNode tank, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = skin => skin.skinParticleSystemEffects.StopEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache3);
        }

        [OnEventFire]
        public void TankVisible(DeactivateTankInvisibilityEffectEvent e, TankParticleNode tankWithEffectNode)
        {
            tankWithEffectNode.particleSystemEffects.StartEmission();
        }

        [OnEventFire]
        public void TankVisible(DeactivateTankInvisibilityEffectEvent e, TankActiveStateNode tank, [JoinByTank] ICollection<SkinParticleNode> skins)
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = skin => skin.skinParticleSystemEffects.StartEmission();
            }
            skins.ForEach<SkinParticleNode>(<>f__am$cache2);
        }

        public class ShaftAimingWorkingStateNode : Node
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingAnimationReadyComponent shaftAimingAnimationReady;
            public SelfComponent self;
        }

        public class SkinParticleNode : Node
        {
            public TankGroupComponent tankGroup;
            public SkinParticleSystemEffectsComponent skinParticleSystemEffects;
        }

        public class TankActiveStateNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankActiveStateComponent tankActiveState;
        }

        public class TankDeadStateNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankDeadStateComponent tankDeadState;
        }

        public class TankParticleNode : Node
        {
            public TankGroupComponent tankGroup;
            public ParticleSystemEffectsComponent particleSystemEffects;
        }

        public class TankWithParticleActiveStateNode : ParticleSystemEffectsSystem.TankParticleNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class TankWithParticleDeadStateNode : ParticleSystemEffectsSystem.TankParticleNode
        {
            public TankDeadStateComponent tankDeadState;
        }
    }
}

