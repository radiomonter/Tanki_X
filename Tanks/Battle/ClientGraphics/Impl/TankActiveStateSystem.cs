namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class TankActiveStateSystem : ECSSystem
    {
        private const float TRANSPARENCY_TRANSITION_TIME = 0.5f;

        [OnEventFire]
        public void InitTransitionFromSemiTransparent(NodeAddedEvent nodeAdded, TankActiveStateNode unit, [Context, JoinByTank] WeaponNode weapon)
        {
            this.SetTransparencyToOpaque(unit);
            this.SetTransparencyToOpaque(weapon);
        }

        private void SetTransparencyToOpaque(RendererNode rendererNode)
        {
            rendererNode.baseRenderer.Renderer.materials = rendererNode.startMaterials.Materials;
            base.ScheduleEvent(new SetTransparencyTransitionDataEvent(ClientGraphicsConstants.SEMI_TRANSPARENT_ALPHA, ClientGraphicsConstants.OPAQUE_ALPHA, 0.5f), rendererNode);
        }

        public class RendererNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
            public StartMaterialsComponent startMaterials;
            public RendererPaintedComponent rendererPainted;
        }

        public class TankActiveStateNode : TankActiveStateSystem.RendererNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class WeaponNode : TankActiveStateSystem.RendererNode
        {
            public WeaponComponent weapon;
        }
    }
}

