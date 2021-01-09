namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankVisualTemperatureControllerSystem : ECSSystem
    {
        [OnEventFire]
        public void AddTargetToBurningBloomPostEffect(NodeAddedEvent e, SingleNode<BurningTargetBloomComponent> bloomPostEffect, [Combine] RendererNode rendererNode, [JoinByTank, Context] SingleNode<TankActiveStateComponent> tank)
        {
            bloomPostEffect.component.burningTargetBloom.targets.Add(rendererNode.baseRenderer.Renderer);
        }

        [OnEventFire]
        public void RemoveTargetFromBurningBloomPostEffect(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> tank, [JoinByTank] ICollection<RendererNode> rendererNodes, [JoinAll] SingleNode<BurningTargetBloomComponent> bloomPostEffect)
        {
            foreach (RendererNode node in rendererNodes)
            {
                bloomPostEffect.component.burningTargetBloom.targets.Remove(node.baseRenderer.Renderer);
            }
        }

        [OnEventFire]
        public void ResetTemperature(NodeAddedEvent evt, TemperatureSemiActiveEffectNode node, [Combine, JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController)
        {
            visualController.component.Temperature = 0f;
            visualController.component.Reset();
        }

        [OnEventFire]
        public void ResetTemperature(NodeRemoveEvent evt, TemperatureDeadEffectNode node, [Combine, JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController)
        {
            visualController.component.Temperature = 0f;
            visualController.component.Reset();
        }

        [OnEventFire]
        public void SetTemperature(NodeAddedEvent evt, TemperatureEffectNode node, [Combine, JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController)
        {
            visualController.component.Temperature = node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature);
        }

        [OnEventFire]
        public void UpdateTemperature(UpdateEvent evt, TemperatureEffectNode node, [Combine, JoinByTank] SingleNode<TemperatureVisualControllerComponent> visualController)
        {
            visualController.component.Temperature = Mathf.Lerp(visualController.component.Temperature, node.logicToVisualTemperatureConverter.ConvertToVisualTemperature(node.temperature.Temperature), evt.DeltaTime);
        }

        public class RendererNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
        }

        public class TemperatureDeadEffectNode : Node
        {
            public TankDeadStateComponent tankDeadState;
            public TemperatureComponent temperature;
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
        }

        public class TemperatureEffectNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TemperatureComponent temperature;
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
        }

        public class TemperatureSemiActiveEffectNode : Node
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
            public TemperatureComponent temperature;
            public LogicToVisualTemperatureConverterComponent logicToVisualTemperatureConverter;
        }
    }
}

