namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections.Generic;

    public class HitTargetAdapter
    {
        public static List<HitTarget> Adapt(List<TargetData> targetsData)
        {
            List<HitTarget> list = new List<HitTarget>();
            for (int i = 0; i < targetsData.Count; i++)
            {
                TargetData targetData = targetsData[i];
                list.Add(Adapt(targetData));
            }
            return list;
        }

        public static HitTarget Adapt(TargetData targetData) => 
            new HitTarget { 
                Entity = targetData.TargetEntity,
                IncarnationEntity = targetData.TargetIncorantionEntity,
                LocalHitPoint = targetData.LocalHitPoint,
                TargetPosition = targetData.TargetPosition,
                HitDirection = targetData.HitDirection,
                HitDistance = targetData.HitDistance
            };
    }
}

