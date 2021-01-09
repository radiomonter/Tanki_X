namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankDeadStateSystem : ECSSystem
    {
        private const float ALPHA_TRANSITION_TIME = 0.5f;

        [OnEventComplete]
        public void FadeTemperature(UpdateEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<RendererNode> renderers)
        {
            float time = (float) ((tank.tankDeadState.EndDate - Date.Now) / (tank.tankDeadState.EndDate - tank.tankDeadStateTexture.FadeStart));
            if (time != tank.tankDeadStateTexture.LastFade)
            {
                tank.tankDeadStateTexture.LastFade = time;
                Vector4 vector = new Vector4(tank.tankDeadStateTexture.HeatEmission.Evaluate(time), 0f, tank.tankDeadStateTexture.WhiteToHeatTexture.Evaluate(time), tank.tankDeadStateTexture.PaintTextureToWhiteHeat.Evaluate(time));
                foreach (RendererNode node in renderers)
                {
                    ClientGraphicsUtil.UpdateVector(node.baseRenderer.Renderer, ClientGraphicsConstants.TEMPERATURE, vector);
                }
            }
        }

        [OnEventFire]
        public void SetFadeStartTime(NodeAddedEvent evt, TankDeadStateNode tank)
        {
            tank.tankDeadStateTexture.FadeStart = Date.Now;
        }

        [OnEventComplete]
        public void StartBeingTranparent(UpdateEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<OpaqueRendererNode> renderers)
        {
            if (Date.Now.AddSeconds(0.5f) >= tank.tankDeadState.EndDate)
            {
                foreach (OpaqueRendererNode node in renderers)
                {
                    node.Entity.AddComponent<TransparentComponent>();
                    base.ScheduleEvent(new SetTransparencyTransitionDataEvent(ClientGraphicsConstants.OPAQUE_ALPHA, ClientGraphicsConstants.TRANSPARENT_ALPHA, 0.5f), node);
                }
            }
        }

        [OnEventComplete]
        public void StopBeingTranparent(NodeRemoveEvent e, TankDeadStateVisibleActivatedNode tank, [JoinByTank] ICollection<TransparnetRendererNode> renderers)
        {
            foreach (TransparnetRendererNode node in renderers)
            {
                node.Entity.RemoveComponent<TransparentComponent>();
            }
        }

        private void SwitchToDeathMaterials(RendererNode renderer)
        {
            renderer.baseRenderer.Renderer.materials = renderer.tankPartMaterialForDeath.DeathMaterials;
        }

        [OnEventFire]
        public void UpdateMaterialsForDeath(NodeAddedEvent e, TankDeadStateNode tank, [Combine, Context, JoinByTank] RendererNode renderer)
        {
            base.ScheduleEvent<TransparencyFinalizeEvent>(renderer);
            this.SwitchToDeathMaterials(renderer);
        }

        [Not(typeof(TransparentComponent))]
        public class OpaqueRendererNode : TankDeadStateSystem.RendererNode
        {
        }

        [Not(typeof(BrokenEffectComponent))]
        public class RendererNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
            public RendererPaintedComponent rendererPainted;
            public TankPartMaterialForDeathComponent tankPartMaterialForDeath;
        }

        public class TankDeadStateNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankDeadStateComponent tankDeadState;
            public TankDeadStateTextureComponent tankDeadStateTexture;
            public TankShaderComponent tankShader;
        }

        public class TankDeadStateVisibleActivatedNode : TankDeadStateSystem.TankDeadStateNode
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class TransparnetRendererNode : TankDeadStateSystem.RendererNode
        {
            public TransparentComponent transparent;
        }
    }
}

