namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftShotValidateSystem : ECSSystem
    {
        [OnEventFire]
        public void SetMask(NodeAddedEvent evt, ShaftIdleNode weapon)
        {
            weapon.shotValidate.UnderGroundValidateMask = LayerMasks.STATIC;
            weapon.shotValidate.RaycastExclusionGameObjects = null;
        }

        [OnEventFire]
        public void SetMask(NodeAddedEvent evt, ShaftWaitingNode weapon, [JoinByTank] TankCollidersNode tank)
        {
            weapon.shotValidate.UnderGroundValidateMask = LayerMasks.VISUAL_STATIC;
            weapon.shotValidate.RaycastExclusionGameObjects = tank.tankColliders.VisualTriggerColliders.ToArray();
        }

        public class ShaftIdleNode : Node
        {
            public ShaftStateControllerComponent shaftStateController;
            public ShaftIdleStateComponent shaftIdleState;
            public ShotValidateComponent shotValidate;
            public TankGroupComponent tankGroup;
        }

        public class ShaftWaitingNode : Node
        {
            public ShaftStateControllerComponent shaftStateController;
            public ShaftWaitingStateComponent shaftWaitingState;
            public ShotValidateComponent shotValidate;
            public TankGroupComponent tankGroup;
        }

        public class TankCollidersNode : Node
        {
            public TankCollidersComponent tankColliders;
            public TankGroupComponent tankGroup;
        }
    }
}

