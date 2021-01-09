namespace Tanks.Battle.ClientGraphics
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class WeldData
    {
        private float percision;
        private long dimension;
        private HashSet<long> weldHashSet = new HashSet<long>();

        public WeldData(float minValue, float maxValue)
        {
            this.percision = 1f / minValue;
            this.dimension = (long) (maxValue * this.percision);
            if (((this.dimension * this.dimension) * this.dimension) > 0x7fffffffffffffffL)
            {
                throw new Exception("maxValue is too big");
            }
        }

        public bool AddValue(Vector3 position) => 
            this.weldHashSet.Add(this.GetWeldHash(position));

        private long GetWeldHash(Vector3 position) => 
            ((((long) (position.x * this.percision)) % this.dimension) + (((((long) (position.y * this.percision)) % this.dimension) * this.dimension) * 2L)) + ((((((long) (position.z * this.percision)) % this.dimension) * this.dimension) * this.dimension) * 4L);
    }
}

