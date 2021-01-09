namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class TargetSectorUtils
    {
        private static bool Between(float position, float left, float right) => 
            (position >= left) && (position <= right);

        public static void CutSectorsByOverlap(LinkedList<TargetSector> targetSectors, float border)
        {
            if (targetSectors.Count >= 2)
            {
                LinkedListNode<TargetSector> first = targetSectors.First;
                LinkedListNode<TargetSector> next = first.Next;
                while (next != null)
                {
                    LinkedListNode<TargetSector> node3 = first.Next;
                    LinkedListNode<TargetSector> node4 = next.Next;
                    CutSectorsNodesByOverlap(first, next, border);
                    TargetSector sector = next.Value;
                    if (sector.Length() < float.Epsilon)
                    {
                        targetSectors.Remove(next);
                    }
                    else if (first.Value.Length() < float.Epsilon)
                    {
                        targetSectors.Remove(first);
                        node4 = null;
                    }
                    next = node4;
                    if ((next == null) && (node3 != null))
                    {
                        first = node3;
                        next = first.Next;
                    }
                }
            }
        }

        private static void CutSectorsNodesByOverlap(LinkedListNode<TargetSector> nodeA, LinkedListNode<TargetSector> nodeB, float border)
        {
            TargetSector sectorA = nodeA.Value;
            TargetSector sectorB = nodeB.Value;
            if (sectorA.Distance > sectorB.Distance)
            {
                nodeA.Value = SubstractSectors(sectorA, sectorB, border);
            }
            else
            {
                nodeB.Value = SubstractSectors(sectorB, sectorA, border);
            }
        }

        private static TargetSector SubstractSectors(TargetSector sectorA, TargetSector sectorB, float border)
        {
            float left = sectorB.Down - border;
            float right = sectorB.Up + border;
            bool flag = Between(sectorA.Down, left, right);
            bool flag2 = Between(sectorA.Up, left, right);
            if (flag && flag2)
            {
                float num3 = 0f;
                sectorA.Up = num3;
                sectorA.Down = num3;
            }
            else if (flag)
            {
                sectorA.Down = right;
            }
            else if (flag2)
            {
                sectorA.Up = left;
            }
            return sectorA;
        }
    }
}

