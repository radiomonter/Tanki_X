namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class TankRendererSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableShadowOnDeadState(NodeRemoveEvent evt, DeadTankNode state, [Combine, JoinByTank] TankPartRendererNode renderer)
        {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            TankMaterialsUtil.SetAlpha(renderer2, 0f);
        }

        [OnEventFire]
        public void HideAndPrepareRenderer(NodeAddedEvent evt, TankPartRendererNode renderer)
        {
            Renderer renderer2 = renderer.baseRenderer.Renderer;
            renderer2.enabled = false;
            TankMaterialsUtil.SetAlpha(renderer2, ClientGraphicsConstants.OPAQUE_ALPHA);
            renderer.startMaterials.Materials = renderer2.materials;
            renderer.Entity.AddComponent<RendererInitializedComponent>();
        }

        [OnEventFire]
        public void HideRenderersVisibleState(NodeRemoveEvent evt, [Combine] RendererReadyForShowingNode renderer, [Context, JoinByTank] AssembledTankNode tank, [Context, JoinByTank] VisibleTankNode state)
        {
            renderer.baseRenderer.Renderer.enabled = false;
        }

        [OnEventFire]
        public void SetColoringTextureToRenderer(NodeAddedEvent evt, AssembledTankNode tank, [JoinByTank, Context] TankRendererInitializedNode tankRendererNode, [JoinByTank, Context] WeaponRendererInitializedNode weaponRendererNode)
        {
            TankMaterialsUtil.ApplyColoring(tankRendererNode.baseRenderer.Renderer, weaponRendererNode.baseRenderer.Renderer, tankRendererNode.coloring, weaponRendererNode.coloring);
            tankRendererNode.Entity.AddComponent<RendererPaintedComponent>();
            weaponRendererNode.Entity.AddComponent<RendererPaintedComponent>();
            tankRendererNode.coloring.transform.localPosition = Vector3.zero;
            weaponRendererNode.coloring.transform.localPosition = Vector3.zero;
            if (tankRendererNode.coloring.overrideEmission)
            {
                tankRendererNode.doubleArmorEffect.usualEmissionColor = tankRendererNode.coloring.emissionColor;
            }
            if (weaponRendererNode.coloring.overrideEmission)
            {
                weaponRendererNode.doubleDamageEffect.usualEmissionColor = weaponRendererNode.coloring.emissionColor;
            }
        }

        [OnEventFire]
        public void SetShadowOnIntersectionWithCamera(NodeAddedEvent evt, IntersectedRenderer renderer)
        {
            renderer.baseRenderer.Renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.baseRenderer.Renderer.receiveShadows = false;
        }

        [OnEventFire]
        public void SetShadowOnIntersectionWithCamera(NodeAddedEvent evt, NotIntersectedRenderer renderer)
        {
            renderer.baseRenderer.Renderer.shadowCastingMode = ShadowCastingMode.On;
            renderer.baseRenderer.Renderer.receiveShadows = true;
        }

        private void SetStartMaterials(RendererReadyForShowingNode renderer)
        {
            renderer.baseRenderer.Renderer.materials = renderer.startMaterials.Materials;
            base.ScheduleEvent<TransparencyFinalizeEvent>(renderer);
        }

        [OnEventFire]
        public void SetStartMaterials(NodeAddedEvent e, TankIncarnationNode incarnation, [Combine, Context, JoinByTank] RendererReadyForShowingNode renderer)
        {
            this.SetStartMaterials(renderer);
        }

        [OnEventFire]
        public void ShowRenderersVisibleState(NodeAddedEvent evt, [Combine] RendererReadyForShowingNode renderer, [Context, JoinByTank] AssembledTankNode tank, [Context, JoinByTank] VisibleTankNode state)
        {
            renderer.baseRenderer.Renderer.enabled = true;
        }

        public class AssembledTankNode : TankRendererSystem.TankNode
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class DeadTankNode : TankRendererSystem.TankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class IntersectedRenderer : TankRendererSystem.TankPartRendererNode
        {
            public TankPartIntersectedWithCameraStateComponent tankPartIntersectedWithCameraState;
        }

        public class NotIntersectedRenderer : TankRendererSystem.TankPartRendererNode
        {
            public TankPartNotIntersectedWithCameraStateComponent tankPartNotIntersectedWithCameraState;
        }

        public class RendererInitializedNode : TankRendererSystem.TankPartRendererNode
        {
            public RendererInitializedComponent rendererInitialized;
        }

        public class RendererReadyForShowingNode : TankRendererSystem.RendererInitializedNode
        {
            public RendererPaintedComponent rendererPainted;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankComponent tank;
        }

        public class TankPartRendererNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
            public StartMaterialsComponent startMaterials;
            public TankPartComponent tankPart;
        }

        public class TankRendererInitializedNode : TankRendererSystem.RendererInitializedNode
        {
            public TankComponent tank;
            public ColoringComponent coloring;
            public DoubleArmorEffectComponent doubleArmorEffect;
        }

        public class VisibleTankNode : TankRendererSystem.TankNode
        {
            public TankVisibleStateComponent tankVisibleState;
        }

        public class WeaponRendererInitializedNode : TankRendererSystem.RendererInitializedNode
        {
            public WeaponComponent weapon;
            public ColoringComponent coloring;
            public DoubleDamageEffectComponent doubleDamageEffect;
        }
    }
}

