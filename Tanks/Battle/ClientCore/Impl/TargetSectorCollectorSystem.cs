namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TargetSectorCollectorSystem : ECSSystem
    {
        public const float TANK_LENGTH_TO_HEIGHT_COEFF = 0.6f;

        private void AddTargetSector(LookAt lookAt, LookTo lookTo, TargetingCone targetingCone, ICollection<TargetSector> sectors, float vDelta, float hDelta)
        {
            Vector3 lhs = lookTo.Position - lookAt.Position;
            float magnitude = lhs.magnitude;
            if ((magnitude <= targetingCone.Distance) && (magnitude >= lookTo.Radius))
            {
                float num2 = (float) ((Math.Asin((double) (lookTo.Radius / magnitude)) * 180.0) / 3.1415926535897931);
                float num3 = num2 + vDelta;
                float num4 = num2 + hDelta;
                float num6 = Vector3.Dot(lhs, lookAt.Forward);
                float num7 = Vector3.Dot(lhs, lookAt.Up);
                float num8 = (float) ((Math.Atan2((double) Vector3.Dot(lhs, lookAt.Left), (double) num6) * 180.0) / 3.1415926535897931);
                if ((num8 >= -(num4 + targetingCone.HAngle)) && (num8 <= (targetingCone.HAngle + num4)))
                {
                    float num9 = (float) ((Math.Atan2((double) num7, (double) num6) * 180.0) / 3.1415926535897931);
                    float num10 = Math.Max(num9 - num3, -targetingCone.VAngleDown);
                    float num11 = Math.Min(num9 + num3, targetingCone.VAngleUp);
                    if (num10 < num11)
                    {
                        TargetSector item = new TargetSector {
                            Down = num10,
                            Up = num11,
                            Distance = magnitude
                        };
                        sectors.Add(item);
                    }
                }
            }
        }

        private float CalculateTankMinimalRadius(Vector3 forward, BoxCollider collider) => 
            collider.size.magnitude * 0.5f;

        [OnEventFire]
        public void CollectTargetSectors(CollectTargetSectorsEvent e, WeaponNode weaponNode, [JoinByBattle] ICollection<TargetTankNode> targetTankNodes, WeaponNode weaponNode1, [JoinByTank] OwnerTankNode ownerTankNode, [JoinByTeam] Optional<TeamNode> team)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weaponNode.muzzlePoint, weaponNode.weaponInstance);
            bool flag = team.IsPresent();
            long num = !flag ? 0L : team.Get().teamGroup.Key;
            LookAt lookAt = new LookAt {
                Position = accessor.GetBarrelOriginWorld(),
                Forward = accessor.GetFireDirectionWorld(),
                Left = accessor.GetLeftDirectionWorld(),
                Up = accessor.GetUpDirectionWorld()
            };
            IEnumerator<TargetTankNode> enumerator = targetTankNodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TargetTankNode current = enumerator.Current;
                if (!ownerTankNode.Entity.Equals(current.Entity) && (!flag || (num != current.Entity.GetComponent<TeamGroupComponent>().Key)))
                {
                    BoxCollider tankToTankCollider = (BoxCollider) current.tankColliders.TankToTankCollider;
                    LookTo lookTo = new LookTo {
                        Position = tankToTankCollider.bounds.center,
                        Radius = this.CalculateTankMinimalRadius(lookAt.Forward, tankToTankCollider)
                    };
                    this.AddTargetSector(lookAt, lookTo, e.TargetingCone, e.TargetSectors, e.VAllowableAngleAcatter, e.HAllowableAngleAcatter);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct LookAt
        {
            public Vector3 Position { get; set; }
            public Vector3 Left { get; set; }
            public Vector3 Forward { get; set; }
            public Vector3 Up { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct LookTo
        {
            public Vector3 Position { get; set; }
            public float Radius { get; set; }
        }

        public class OwnerTankNode : Node
        {
            public TankComponent tank;
        }

        public class TargetTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public TankCollidersComponent tankColliders;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
        }

        public class WeaponNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

