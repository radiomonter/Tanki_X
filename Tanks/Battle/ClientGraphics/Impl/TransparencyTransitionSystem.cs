namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;

    public class TransparencyTransitionSystem : ECSSystem
    {
        [OnEventComplete]
        public void FinalizeTransparency(TransparencyFinalizeEvent evt, TransitionRendererNode renderer, [JoinByTank] TankShaderNode tankShader)
        {
            renderer.Entity.RemoveComponent<TransparencyTransitionComponent>();
            TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, ClientGraphicsConstants.OPAQUE_ALPHA);
            base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.TRANSPARENCY_TRANSITION_EFFECT, false), tankShader);
        }

        [OnEventComplete]
        public void InitTransparency(TransparencyInitEvent evt, RendererNode renderer, [JoinByTank] TankNotInvisibilityInvisibilityShaderNode tankShader)
        {
            ClientGraphicsUtil.ApplyShaderToRenderer(renderer.baseRenderer.Renderer, tankShader.tankShader.TransparentShader);
        }

        [OnEventFire]
        public void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, NotTransitionRendererNode renderer)
        {
            TransparencyTransitionComponent transparencyTransition = new TransparencyTransitionComponent();
            this.SetTransparencyTransitionData(evt, renderer, transparencyTransition);
            renderer.Entity.AddComponent(transparencyTransition);
            base.ScheduleEvent<TransparencyInitEvent>(renderer.Entity);
        }

        [OnEventFire]
        public void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, TransitionRendererNode renderer)
        {
            this.SetTransparencyTransitionData(evt, renderer, renderer.transparencyTransition);
            base.ScheduleEvent<TransparencyInitEvent>(renderer.Entity);
        }

        private void SetTransparencyTransitionData(SetTransparencyTransitionDataEvent evt, RendererNode renderer, TransparencyTransitionComponent transparencyTransition)
        {
            transparencyTransition.OriginAlpha = evt.OriginAlpha;
            transparencyTransition.TargetAlpha = evt.TargetAlpha;
            transparencyTransition.TransparencyTransitionTime = evt.TransparencyTransitionTime;
            renderer.baseRenderer.Renderer.enabled = true;
            transparencyTransition.AlphaSpeed = (transparencyTransition.TargetAlpha - transparencyTransition.OriginAlpha) / transparencyTransition.TransparencyTransitionTime;
            transparencyTransition.CurrentAlpha = transparencyTransition.OriginAlpha;
        }

        [OnEventFire]
        public void UpdateTransparencyTransition(TimeUpdateEvent evt, TransitionRendererNode renderer)
        {
            TransparencyTransitionComponent transparencyTransition = renderer.transparencyTransition;
            transparencyTransition.CurrentTransitionTime += evt.DeltaTime;
            if (renderer.baseRenderer.Renderer)
            {
                float targetAlpha;
                if (transparencyTransition.CurrentTransitionTime < transparencyTransition.TransparencyTransitionTime)
                {
                    targetAlpha = transparencyTransition.OriginAlpha + (transparencyTransition.AlphaSpeed * transparencyTransition.CurrentTransitionTime);
                }
                else
                {
                    targetAlpha = transparencyTransition.TargetAlpha;
                    if (transparencyTransition.TargetAlpha >= ClientGraphicsConstants.OPAQUE_ALPHA)
                    {
                        base.ScheduleEvent<TransparencyFinalizeEvent>(renderer.Entity);
                    }
                    else if (transparencyTransition.TargetAlpha <= ClientGraphicsConstants.TRANSPARENT_ALPHA)
                    {
                        renderer.baseRenderer.Renderer.enabled = false;
                    }
                }
                renderer.transparencyTransition.CurrentAlpha = targetAlpha;
                TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, targetAlpha);
            }
        }

        [Not(typeof(TransparencyTransitionComponent))]
        public class NotTransitionRendererNode : TransparencyTransitionSystem.RendererNode
        {
        }

        public class RendererNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
        }

        [Not(typeof(TankInvisibilityEffectActivationStateComponent)), Not(typeof(TankInvisibilityEffectWorkingStateComponent)), Not(typeof(TankInvisibilityEffectDeactivationStateComponent))]
        public class TankNotInvisibilityInvisibilityShaderNode : TransparencyTransitionSystem.TankShaderNode
        {
        }

        public class TankShaderNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankShaderComponent tankShader;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
        }

        public class TransitionRendererNode : TransparencyTransitionSystem.RendererNode
        {
            public TransparencyTransitionComponent transparencyTransition;
        }
    }
}

