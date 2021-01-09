namespace Tanks.Battle.ClientGraphics
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class BonusRegionShowSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateBonusRegionMaterialComponent(NodeAddedEvent evt, SingleNode<BonusRegionInstanceComponent> bonusRegion)
        {
            bonusRegion.Entity.AddComponent(new MaterialComponent(MaterialAlphaUtils.GetMaterial(bonusRegion.component.BonusRegionInstance)));
        }

        [OnEventFire]
        public void SetRegionTransparent(NodeAddedEvent e, InvisibleBonusRegionNode region)
        {
            region.opacityBonusRegion.Opacity = 0f;
        }

        [OnEventFire]
        public void SetRegionTransparent(NodeRemoveEvent e, VisibleBonusRegionNode region)
        {
            region.opacityBonusRegion.Opacity = 0f;
        }

        [OnEventComplete]
        public void UpdateRegionOpacity(TimeUpdateEvent e, BonusRegionNode node, [JoinAll] SingleNode<BonusRegionClientConfigComponent> configNode)
        {
            Material material = node.material.Material;
            float alpha = material.GetAlpha();
            float num2 = e.DeltaTime * configNode.component.opacityChangingSpeed;
            material.SetAlpha(Mathf.Clamp(node.opacityBonusRegion.Opacity, alpha - num2, alpha + num2));
        }

        [OnEventFire]
        public void UpdateRegionOpacityByDistance(TimeUpdateEvent e, TankNode tank, [JoinAll, Combine] VisibleBonusRegionNode region, [JoinAll] SingleNode<BonusRegionClientConfigComponent> configNode, [JoinAll] SingleNode<RoundActiveStateComponent> round)
        {
            BonusRegionClientConfigComponent component = configNode.component;
            float num = Vector3.Distance(tank.tankColliders.BoundsCollider.transform.position, region.spatialGeometry.Position);
            region.opacityBonusRegion.Opacity = Mathf.Clamp01(1f - ((num - component.maxOpacityRadius) / (component.minOpacityRadius - component.maxOpacityRadius)));
        }

        public class BonusRegionNode : Node
        {
            public BonusRegionComponent bonusRegion;
            public SpatialGeometryComponent spatialGeometry;
            public OpacityBonusRegionComponent opacityBonusRegion;
            public BonusRegionInstanceComponent bonusRegionInstance;
            public MaterialComponent material;
        }

        [Not(typeof(VisibleBonusRegionComponent))]
        public class InvisibleBonusRegionNode : Node
        {
            public BonusRegionComponent bonusRegion;
            public SpatialGeometryComponent spatialGeometry;
            public OpacityBonusRegionComponent opacityBonusRegion;
        }

        public class TankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankCollidersComponent tankColliders;
            public TankMovableComponent tankMovable;
        }

        public class VisibleBonusRegionNode : Node
        {
            public BonusRegionComponent bonusRegion;
            public SpatialGeometryComponent spatialGeometry;
            public VisibleBonusRegionComponent visibleBonusRegion;
            public OpacityBonusRegionComponent opacityBonusRegion;
        }
    }
}

