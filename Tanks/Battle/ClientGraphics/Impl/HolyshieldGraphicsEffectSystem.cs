namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class HolyshieldGraphicsEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitTankHolyshieldEffect(NodeAddedEvent e, TankHolyshieldGraphicsEffectNode tank, [JoinAll] Optional<SingleNode<SelfTankComponent>> selfTank)
        {
            GameObject obj2 = tank.newHolyshieldEffect.InitEffect(tank.tankVisualRoot.transform, tank.baseRenderer.Renderer as SkinnedMeshRenderer, Layers.VISUAL_STATIC);
            obj2.AddComponent<Rigidbody>().isKinematic = true;
            if (selfTank.IsPresent())
            {
                obj2.AddComponent<HolyshiedTargetBehaviour>().Init(selfTank.Get().Entity, null);
            }
            tank.Entity.AddComponent<TankHolyshieldEffectReadyComponent>();
        }

        [OnEventFire]
        public void Play(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] TankReadyHolyshieldGraphicsEffectNode tank)
        {
            tank.newHolyshieldEffect.Play();
        }

        [OnEventFire]
        public void Stop(NodeRemoveEvent e, HolyshieldEffectNode effect, [JoinByTank] TankReadyHolyshieldGraphicsEffectNode tank)
        {
            tank.newHolyshieldEffect.Stop();
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] SelfTankNode tank, [JoinByTank, Context] IdleShaftNode shaft)
        {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] SelfTankNode tank, [JoinByTank, Context] ShaftAimingWorkActivationNode shaft)
        {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] SelfTankNode tank, [JoinByTank, Context] ShaftAimingWorkFinishNode shaft)
        {
            tank.newHolyshieldEffect.UpdateAlpha(0f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] SelfTankNode tank, [JoinByTank, Context] ShaftAimingWorkingNode shaft)
        {
            tank.newHolyshieldEffect.UpdateAlpha(0f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(NodeAddedEvent e, HolyshieldEffectNode effect, [JoinByTank, Context] SelfTankNode tank, [JoinByTank, Context] WaitingShaftNode shaft)
        {
            tank.newHolyshieldEffect.UpdateAlpha(1f);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(UpdateEvent e, HolyshieldEffectNode effect, [JoinByTank] SelfTankNode tank, [JoinByTank] ShaftAimingWorkActivationNode shaft)
        {
            float num = Mathf.Clamp01(shaft.shaftAimingWorkActivationState.ActivationTimer / shaft.shaftStateConfig.ActivationToWorkingTransitionTimeSec);
            tank.newHolyshieldEffect.UpdateAlpha(1f - num);
        }

        [OnEventFire]
        public void UpdateEffectVisibility(UpdateEvent e, HolyshieldEffectNode effect, [JoinByTank] SelfTankNode tank, [JoinByTank] ShaftAimingWorkFinishNode shaft)
        {
            float alpha = Mathf.Clamp01(shaft.shaftAimingWorkFinishState.FinishTimer / shaft.shaftStateConfig.FinishToIdleTransitionTimeSec);
            tank.newHolyshieldEffect.UpdateAlpha(alpha);
        }

        public class HolyshieldEffectNode : Node
        {
            public InvulnerabilityEffectComponent invulnerabilityEffect;
            public HolyshieldEffectComponent holyshieldEffect;
            public TankGroupComponent tankGroup;
        }

        public class IdleShaftNode : HolyshieldGraphicsEffectSystem.ShaftNode
        {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class SelfTankNode : HolyshieldGraphicsEffectSystem.TankReadyHolyshieldGraphicsEffectNode
        {
            public SelfTankComponent selfTank;
        }

        public class ShaftAimingWorkActivationNode : HolyshieldGraphicsEffectSystem.ShaftNode
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
        }

        public class ShaftAimingWorkFinishNode : HolyshieldGraphicsEffectSystem.ShaftNode
        {
            public ShaftAimingWorkFinishStateComponent shaftAimingWorkFinishState;
        }

        public class ShaftAimingWorkingNode : HolyshieldGraphicsEffectSystem.ShaftNode
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class ShaftNode : Node
        {
            public TankGroupComponent tankGroup;
            public ShaftComponent shaft;
            public ShaftStateConfigComponent shaftStateConfig;
        }

        public class TankHolyshieldGraphicsEffectNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public NewHolyshieldEffectComponent newHolyshieldEffect;
            public TankVisualRootComponent tankVisualRoot;
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
            public VisualMountPointComponent visualMountPoint;
        }

        public class TankReadyHolyshieldGraphicsEffectNode : HolyshieldGraphicsEffectSystem.TankHolyshieldGraphicsEffectNode
        {
            public TankHolyshieldEffectReadyComponent tankHolyshieldEffectReady;
        }

        public class WaitingShaftNode : HolyshieldGraphicsEffectSystem.ShaftNode
        {
            public ShaftWaitingStateComponent shaftWaitingState;
        }
    }
}

