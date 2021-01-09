namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class PaintGraphicsBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void OnNodeAdded(NodeAddedEvent evt, PaintGraphicsNode paintGraphics)
        {
            Entity entity = paintGraphics.Entity;
            paintGraphics.tankPartPaintInstance.PaintInstance.GetComponent<EntityBehaviour>().BuildEntity(entity);
        }

        public class PaintGraphicsNode : Node
        {
            public TankPartPaintInstanceComponent tankPartPaintInstance;
        }
    }
}

