namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WallhackSystem : ECSSystem
    {
        [OnEventComplete]
        public void check(AfterFixedUpdateEvent e, TankNode tank)
        {
            if (tank.tankMovementSender.LastSentMovement != null)
            {
                Vector3 position = tank.tankMovementSender.LastSentMovement.Value.Position;
                Vector3 vector3 = tank.rigidbody.Rigidbody.worldCenterOfMass - position;
                if ((vector3.sqrMagnitude >= 0.0001) && Physics.SphereCast(new Ray(position, vector3.normalized), 0.04f, vector3.magnitude, LayerMasks.STATIC))
                {
                    base.ScheduleEvent<SendTankMovementEvent>(tank);
                }
            }
        }

        public class TankNode : Node
        {
            public TankSyncComponent tankSync;
            public RigidbodyComponent rigidbody;
            public TankMovementSenderComponent tankMovementSender;
        }
    }
}

