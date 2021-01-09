namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class FlyingTankSystem : ECSSystem
    {
        private const float MINIMAL_HEIGHT = 0.02f;

        [OnEventFire]
        public void Fall(UpdateEvent e, FlyingTank tank)
        {
            RaycastHit hit;
            Ray ray = new Ray {
                origin = tank.rigidbody.Rigidbody.position,
                direction = Vector3.down
            };
            if (Physics.Raycast(ray, out hit, 0.02f, (1 << (Layers.STATIC & 0x1f)) | (1 << (Layers.VISUAL_STATIC & 0x1f))))
            {
                base.ScheduleEvent<FlyingTankGroundedEvent>(tank);
            }
        }

        public class FlyingTank : Node
        {
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public FlyingTankComponent flyingTank;
        }
    }
}

