namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CandidatesComparer : Comparer<Submesh>
    {
        public Vector3 center;

        public override int Compare(Submesh a, Submesh b)
        {
            if (a == null)
            {
                return ((b != null) ? -1 : 0);
            }
            if (b == null)
            {
                return 1;
            }
            float num5 = (a.bounds.center - this.center).sqrMagnitude + a.bounds.extents.sqrMagnitude;
            return num5.CompareTo((float) ((b.bounds.center - this.center).sqrMagnitude + b.bounds.extents.sqrMagnitude));
        }
    }
}

