namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SectorDirectionsCollectorSystem : AbstractDirectionsCollectorSystem
    {
        private void CollectSectorDirections(TargetingData targetingData, Vector3 rotationAxis, TargetSector targetSector, int numRays)
        {
            numRays = ((numRays / 2) * 2) + 1;
            float num = (targetSector.Up + targetSector.Down) * 0.5f;
            float num2 = targetSector.Length() / ((float) (numRays - 1));
            float num3 = 0f;
            for (int i = 0; i < numRays; i++)
            {
                Vector3 dir = (Vector3) (Quaternion.AngleAxis(num + num3, rotationAxis) * targetingData.Dir);
                CollectDirection(targetingData.Origin, dir, Mathf.Abs((float) (num + num3)), targetingData);
                num3 = ((i % 2) != 0) ? -num3 : (Math.Abs(num3) + num2);
            }
        }

        [OnEventFire]
        public void CollectSectorDirectionsEvent(Tanks.Battle.ClientCore.API.CollectSectorDirectionsEvent evt, VerticalSectorTargetingNode verticalSectorTargeting, [JoinByTank] WeaponNode weapon)
        {
            TargetingData targetingData = evt.TargetingData;
            VerticalSectorsTargetingComponent verticalSectorsTargeting = verticalSectorTargeting.verticalSectorsTargeting;
            CollectDirection(evt.TargetingData.Origin, evt.TargetingData.Dir, 0f, targetingData);
            if (evt.TargetSectors.Count != 0)
            {
                Vector3 leftDirectionWorld = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetLeftDirectionWorld();
                TargetSectorUtils.CutSectorsByOverlap((LinkedList<TargetSector>) evt.TargetSectors, 1f / verticalSectorsTargeting.RaysPerDegree);
                IEnumerator<TargetSector> enumerator = evt.TargetSectors.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TargetSector current = enumerator.Current;
                    float num = current.Length();
                    int numRays = (int) Math.Floor((double) (verticalSectorsTargeting.RaysPerDegree * num));
                    this.CollectSectorDirections(targetingData, leftDirectionWorld, current, numRays);
                }
            }
        }

        public class VerticalSectorTargetingNode : Node
        {
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
        }

        public class WeaponNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

