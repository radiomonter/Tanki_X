namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;

    public class TankSemiActiveStateSystem : ECSSystem
    {
        [OnEventFire]
        public void CloseSemiActiveTransition(NodeRemoveEvent e, SemiActiveTankNode tank, [Combine, JoinByTank] SingleNode<BaseRendererComponent> renderer)
        {
            base.ScheduleEvent<TransparencyFinalizeEvent>(renderer);
        }

        [OnEventFire]
        public void InitSemiActiveTransition(NodeAddedEvent e, SemiActiveTankNode tank, [Combine, Context, JoinByTank] RendererNode renderer)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TRANSPARENCY_TRANSITION_EFFECT, false), tank);
            base.ScheduleEvent<TransparencyInitEvent>(renderer);
            TankMaterialsUtil.SetAlpha(renderer.baseRenderer.Renderer, ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA);
        }

        public class RendererNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
            public TankPartComponent tankPart;
            public RendererPaintedComponent rendererPainted;
        }

        public class SemiActiveTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankSemiActiveStateComponent tankSemiActiveState;
            public TankShaderComponent tankShader;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
        }
    }
}

