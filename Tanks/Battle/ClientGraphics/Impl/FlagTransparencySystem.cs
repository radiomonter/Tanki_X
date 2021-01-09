namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class FlagTransparencySystem : ECSSystem
    {
        [OnEventFire]
        public void DropFlag(NodeAddedEvent e, DroppedFlagNode flagNode)
        {
            setAlpha(flagNode.flagInstance, 1f);
        }

        [OnEventFire]
        public void PickupFlag(NodeAddedEvent e, CarriedFlagNode flagNode)
        {
            setAlpha(flagNode.flagInstance, 0.5f);
        }

        private static void setAlpha(FlagInstanceComponent flagInstanceComponent, float val)
        {
            flagInstanceComponent.FlagInstance.GetComponent<Sprite3D>().material.SetFloat("_Alpha", val);
        }

        public class CarriedFlagNode : Node
        {
            public FlagInstanceComponent flagInstance;
            public TankGroupComponent tankGroup;
        }

        public class DroppedFlagNode : Node
        {
            public FlagInstanceComponent flagInstance;
            public FlagGroundedStateComponent flagGroundedState;
        }
    }
}

