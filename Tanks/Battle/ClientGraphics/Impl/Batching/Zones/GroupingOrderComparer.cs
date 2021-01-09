namespace Tanks.Battle.ClientGraphics.Impl.Batching.Zones
{
    using System;
    using System.Collections.Generic;

    public class GroupingOrderComparer : Comparer<Submesh>
    {
        public override int Compare(Submesh a, Submesh b) => 
            (a != null) ? ((b != null) ? -a.nearValue.CompareTo(b.nearValue) : 1) : ((b != null) ? -1 : 0);
    }
}

