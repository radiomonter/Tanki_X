namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class TankCommonBuilderSystem : ECSSystem
    {
        [OnEventComplete]
        public void OnNodeAdded(NodeAddedEvent evt, TankCommonGraphicsNode tankCommonGraphics)
        {
            Entity entity = tankCommonGraphics.Entity;
            tankCommonGraphics.tankCommonInstance.TankCommonInstance.GetComponent<EntityBehaviour>().BuildEntity(entity);
        }

        public class TankCommonGraphicsNode : Node
        {
            public TankCommonInstanceComponent tankCommonInstance;
        }
    }
}

