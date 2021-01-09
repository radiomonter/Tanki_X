namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ShaftAimingRendererSystem : ECSSystem
    {
        [OnEventFire]
        public void ProcessReducing(UpdateEvent evt, AimingWorkActivationAlphaTransitionNode weapon, [JoinByTank] TankNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            float t = weapon.shaftAimingWorkActivationState.ActivationTimer / weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec;
            float alpha = Mathf.Lerp(weapon.shaftAimingRendererReducingAlpha.InitialAlpha, ClientGraphicsConstants.TRANSPARENT_ALPHA, t);
            this.SetTransparentMode(renderers, tank, null, alpha);
        }

        [OnEventFire]
        public void RecoverAlpha(TimeUpdateEvent evt, AimingRecoveringNode weapon, [JoinByTank] TankNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            float num4;
            float num3 = TankMaterialsUtil.GetAlpha(tank.trackRenderer.Renderer) + (weapon.shaftAimingRendererEffect.AlphaRecoveringSpeed * evt.DeltaTime);
            if (num3 < ClientGraphicsConstants.OPAQUE_ALPHA)
            {
                num4 = num3;
                this.SetTransparentMode(renderers, tank, null, num4);
            }
            else
            {
                num4 = ClientGraphicsConstants.OPAQUE_ALPHA;
                this.SetTransparentMode(renderers, tank, null, num4);
                base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.SHAFT_AIMING_EFFECT, false), tank);
                weapon.Entity.RemoveComponent<ShaftAimingRendererRecoveringAlphaComponent>();
            }
        }

        [OnEventFire]
        public void SetOpaqueModeOnExitTankActiveState(NodeRemoveEvent evt, TankNode tank, [JoinByTank] ShaftAimingRendererEffectNode weapon, [JoinByTank] ICollection<RendererNode> renderers)
        {
            float alpha = ClientGraphicsConstants.OPAQUE_ALPHA;
            this.SetTransparentMode(renderers, tank, null, alpha);
            base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.SHAFT_AIMING_EFFECT, false), tank);
        }

        private void SetTransparentMode(ICollection<RendererNode> renderers, TankNode tank, Shader targetShader = null, float alpha = -1f)
        {
            foreach (RendererNode node in renderers)
            {
                if (targetShader != null)
                {
                    ClientGraphicsUtil.ApplyShaderToRenderer(node.baseRenderer.Renderer, targetShader);
                }
                if ((alpha == 0f) || tank.Entity.HasComponent<TankPartIntersectedWithCameraStateComponent>())
                {
                    node.baseRenderer.Renderer.enabled = false;
                }
                else if (alpha > 0f)
                {
                    TankMaterialsUtil.SetAlpha(node.baseRenderer.Renderer, alpha);
                    node.baseRenderer.Renderer.enabled = true;
                }
            }
        }

        [OnEventFire]
        public void StartReducing(NodeAddedEvent evt, AimingWorkActivationNode weapon, [JoinByTank] TankActivationInvisibilityEffectNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            this.StartReducing(weapon, tank, renderers, false);
        }

        [OnEventFire]
        public void StartReducing(NodeAddedEvent evt, AimingWorkActivationNode weapon, [JoinByTank] TankDeactivationInvisibilityEffectNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            this.StartReducing(weapon, tank, renderers, false);
        }

        [OnEventFire]
        public void StartReducing(NodeAddedEvent evt, AimingWorkActivationNode weapon, [JoinByTank] TankNotInvisibilityEffectNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            this.StartReducing(weapon, tank, renderers, true);
        }

        [OnEventFire]
        public void StartReducing(NodeAddedEvent evt, AimingWorkActivationNode weapon, [JoinByTank] TankWorkingInvisibilityEffectNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            this.StartReducing(weapon, tank, renderers, false);
        }

        private void StartReducing(AimingWorkActivationNode weapon, TankNode tank, ICollection<RendererNode> renderers, bool switchShader)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.SHAFT_AIMING_EFFECT, false), tank);
            if (switchShader)
            {
                Shader transparentShader = tank.tankShader.TransparentShader;
                this.SetTransparentMode(renderers, tank, transparentShader, -1f);
            }
            if (weapon.Entity.HasComponent<ShaftAimingRendererRecoveringAlphaComponent>())
            {
                weapon.Entity.RemoveComponent<ShaftAimingRendererRecoveringAlphaComponent>();
            }
            float alpha = TankMaterialsUtil.GetAlpha(tank.trackRenderer.Renderer);
            weapon.Entity.AddComponent(new ShaftAimingRendererReducingAlphaComponent(alpha));
            ShaftAimingRendererQueueMapComponent component = new ShaftAimingRendererQueueMapComponent();
            foreach (RendererNode node in renderers)
            {
                foreach (Material material in node.baseRenderer.Renderer.materials)
                {
                    component.QueueMap.Add(material, material.renderQueue);
                    material.renderQueue = weapon.shaftAimingRendererEffect.TransparentRenderQueue;
                }
            }
            weapon.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void SwitchAlphaMode(NodeAddedEvent evt, AimingIdleReducingNode weapon)
        {
            weapon.Entity.RemoveComponent<ShaftAimingRendererReducingAlphaComponent>();
            foreach (Material material in weapon.shaftAimingRendererQueueMap.QueueMap.Keys)
            {
                material.renderQueue = weapon.shaftAimingRendererQueueMap.QueueMap[material];
            }
            weapon.Entity.RemoveComponent<ShaftAimingRendererQueueMapComponent>();
            weapon.Entity.AddComponent<ShaftAimingRendererRecoveringAlphaComponent>();
        }

        public class AimingIdleReducingNode : Node
        {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public ShaftAimingRendererReducingAlphaComponent shaftAimingRendererReducingAlpha;
            public ShaftAimingRendererQueueMapComponent shaftAimingRendererQueueMap;
            public TankGroupComponent tankGroup;
        }

        public class AimingRecoveringNode : Node
        {
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingRendererRecoveringAlphaComponent shaftAimingRendererRecoveringAlpha;
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public TankGroupComponent tankGroup;
        }

        public class AimingWorkActivationAlphaTransitionNode : Node
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingRendererReducingAlphaComponent shaftAimingRendererReducingAlpha;
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public ShaftStateConfigComponent shaftStateConfig;
            public TankGroupComponent tankGroup;
        }

        public class AimingWorkActivationNode : Node
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public TankGroupComponent tankGroup;
        }

        public class RendererNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
            public TankPartComponent tankPart;
        }

        public class ShaftAimingRendererEffectNode : Node
        {
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingRendererEffectComponent shaftAimingRendererEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankActivationInvisibilityEffectNode : ShaftAimingRendererSystem.TankNode
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class TankDeactivationInvisibilityEffectNode : ShaftAimingRendererSystem.TankNode
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class TankNode : Node
        {
            public TankShaderComponent tankShader;
            public TrackRendererComponent trackRenderer;
            public TankGroupComponent tankGroup;
            public TankComponent tank;
            public TankActiveStateComponent tankActiveState;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
        }

        [Not(typeof(TankInvisibilityEffectWorkingStateComponent)), Not(typeof(TankInvisibilityEffectDeactivationStateComponent)), Not(typeof(TankInvisibilityEffectActivationStateComponent))]
        public class TankNotInvisibilityEffectNode : ShaftAimingRendererSystem.TankNode
        {
        }

        public class TankWorkingInvisibilityEffectNode : ShaftAimingRendererSystem.TankNode
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }
    }
}

