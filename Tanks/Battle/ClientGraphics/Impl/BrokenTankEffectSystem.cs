namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class BrokenTankEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableOrEnable(NodeAddedEvent e, BrokenNode brokenPart)
        {
            if (GraphicsSettings.INSTANCE.CurrentQualityLevel < 2)
            {
                brokenPart.Entity.RemoveComponent<BrokenEffectComponent>();
            }
            else
            {
                brokenPart.brokenEffect.Init();
            }
        }

        [OnEventComplete]
        public void StartEffect(NodeAddedEvent e, DeadTankNode deadTankNode, [JoinByTank, Combine] BrokenNode brokenPart)
        {
            Shader transparentShader = deadTankNode.tankShader.TransparentShader;
            brokenPart.brokenEffect.StartEffect(deadTankNode.assembledTank.AssemblyRoot, deadTankNode.rigidbody.Rigidbody, brokenPart.baseRenderer.Renderer, transparentShader, (deadTankNode.temperatureVisualController.Temperature < 0f) ? ((float) 1) : ((float) 20));
            brokenPart.baseRenderer.Renderer.enabled = false;
        }

        public class BrokenNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public BrokenEffectComponent brokenEffect;
        }

        public class DeadTankNode : Node
        {
            public TankComponent tank;
            public TankDeadStateComponent tankDeadState;
            public TankShaderComponent tankShader;
            public AssembledTankComponent assembledTank;
            public RigidbodyComponent rigidbody;
            public TemperatureVisualControllerComponent temperatureVisualController;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
        }
    }
}

