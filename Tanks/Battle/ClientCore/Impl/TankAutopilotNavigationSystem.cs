namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;
    using UnityEngine.AI;

    public class TankAutopilotNavigationSystem : ECSSystem
    {
        private static float SELF_DESTRUCT_ON_UNDEGROUND_PROBABILITY = 0.01f;
        private static float PREFARE_ATTACKING_RANGE = 15f;

        [OnEventFire]
        public void AddNavigationComponentToTank(NodeAddedEvent e, SingleNode<TankAutopilotComponent> tank)
        {
            tank.Entity.AddComponent(new NavigationDataComponent());
        }

        [OnEventFire]
        public void Control(FixedUpdateEvent e, AutopilotTankNode tank, [JoinByTank] WeaponNode weapon)
        {
            if (tank.rigidbody.Rigidbody)
            {
                Vector3 destination;
                if (!tank.autopilotMovementController.MoveToTarget)
                {
                    destination = tank.autopilotMovementController.Destination;
                }
                else
                {
                    Entity target = tank.autopilotMovementController.Target;
                    if ((target == null) && tank.autopilotMovementController.MoveToTarget)
                    {
                        return;
                    }
                    if (!target.HasComponent<RigidbodyComponent>() || !target.HasComponent<TankCollidersComponent>())
                    {
                        return;
                    }
                    AutopilotMovementControllerComponent autopilotMovementController = tank.autopilotMovementController;
                    destination = target.GetComponent<RigidbodyComponent>().Rigidbody.position;
                }
                if (tank.navigationData.PathData == null)
                {
                    PathData data = new PathData {
                        timeToRecalculatePath = Time.timeSinceLevelLoad
                    };
                    tank.navigationData.PathData = data;
                }
                this.ControlMove(tank, destination);
            }
        }

        private void ControlMove(AutopilotTankNode tank, Vector3 targetPosition)
        {
            TankAutopilotComponent tankAutopilot = tank.tankAutopilot;
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            Vector3 position = rigidbody.transform.position;
            bool flag = rigidbody.velocity.magnitude < 0.5f;
            if (tank.autopilotMovementController.Moving)
            {
                this.MoveToTarget(tank.navigationData, position, targetPosition, rigidbody);
            }
        }

        private bool CurrentPointReached(Vector3 currentPosision, Vector3 currentPoint, Rigidbody rigidbody)
        {
            float num = Vector3.Dot((currentPoint - currentPosision).normalized, rigidbody.transform.forward);
            return ((num > -0.2) && (num < 0.2f));
        }

        private void MoveToTarget(NavigationDataComponent autopilot, Vector3 currentPosition, Vector3 targetPosition, Rigidbody rigidbody)
        {
            PathData pathData = autopilot.PathData;
            if (this.TimeToRecalculatePath(pathData) || (pathData.currentPathIndex >= (pathData.currentPath.Length - 3)))
            {
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(currentPosition, targetPosition, -1, path);
                if (path.corners.Length > 1)
                {
                    pathData.currentPath = path.corners;
                    pathData.currentPathIndex = 1;
                    autopilot.MovePosition = pathData.currentPath[pathData.currentPathIndex];
                    autopilot.PathData.timeToRecalculatePath = Time.timeSinceLevelLoad + Random.Range((float) 0.5f, (float) 2f);
                }
            }
            else if (this.CurrentPointReached(currentPosition, pathData.currentPath[pathData.currentPathIndex], rigidbody))
            {
                try
                {
                    pathData.currentPathIndex += 2;
                    autopilot.MovePosition = pathData.currentPath[pathData.currentPathIndex];
                }
                catch (Exception)
                {
                    object[] objArray1 = new object[] { "Index out of range! current index ", pathData.currentPathIndex, " array lenght", pathData.currentPath.Length };
                    Debug.LogWarning(string.Concat(objArray1));
                }
            }
            try
            {
                int currentPathIndex = pathData.currentPathIndex;
                while (true)
                {
                    if (currentPathIndex >= (pathData.currentPath.Length - 2))
                    {
                        for (int i = 0; i < (pathData.currentPathIndex - 1); i++)
                        {
                        }
                        break;
                    }
                    currentPathIndex++;
                }
            }
            catch (Exception)
            {
            }
        }

        [OnEventFire]
        public void SelfDestroy(FixedUpdateEvent e, AutopilotTankNode tank, [JoinByTank] SingleNode<WeaponUndergroundComponent> weapon)
        {
            if (Random.value < SELF_DESTRUCT_ON_UNDEGROUND_PROBABILITY)
            {
                tank.Entity.AddComponentIfAbsent<SelfDestructionComponent>();
            }
        }

        private bool TimeToRecalculatePath(PathData pathData) => 
            pathData.timeToRecalculatePath <= Time.timeSinceLevelLoad;

        public class AutopilotTankNode : Node
        {
            public TankSyncComponent tankSync;
            public TankAutopilotComponent tankAutopilot;
            public TankActiveStateComponent tankActiveState;
            public NavigationDataComponent navigationData;
            public RigidbodyComponent rigidbody;
            public ChassisComponent chassis;
            public TankCollidersComponent tankColliders;
            public AutopilotMovementControllerComponent autopilotMovementController;
            public AutopilotWeaponControllerComponent autopilotWeaponController;
        }

        public class MapComponent : Node
        {
            public MapInstanceComponent mapInstance;
        }

        public class TankNode : Node
        {
            public RigidbodyComponent rigidbody;
            public TankCollidersComponent tankColliders;
            public AssembledTankComponent assembledTank;
        }

        public class WeaponNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public MuzzlePointComponent muzzlePoint;
        }
    }
}

