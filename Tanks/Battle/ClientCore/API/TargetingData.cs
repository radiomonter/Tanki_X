namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TargetingData
    {
        [CompilerGenerated]
        private static Func<DirectionData, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<DirectionData, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<DirectionData, bool> <>f__am$cache2;

        public TargetingData()
        {
            this.Directions = new List<DirectionData>(10);
        }

        public bool HasAnyHit()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = direction => direction.HasAnyHit();
            }
            return this.Directions.Any<DirectionData>(<>f__am$cache0);
        }

        public bool HasBaseStaticHit() => 
            this.Directions.First<DirectionData>().HasStaticHit();

        public bool HasStaticHit()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = direction => direction.HasStaticHit();
            }
            return this.Directions.Any<DirectionData>(<>f__am$cache2);
        }

        public bool HasTargetHit()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = direction => direction.HasTargetHit();
            }
            return this.Directions.Any<DirectionData>(<>f__am$cache1);
        }

        public TargetingData Init()
        {
            this.Directions.Clear();
            this.Origin = Vector3.zero;
            this.Dir = Vector3.zero;
            this.FullDistance = 0f;
            this.MaxAngle = 0f;
            this.BestDirection = null;
            this.BestDirectionIndex = 0;
            return this;
        }

        public Vector3 Origin { get; set; }

        public Vector3 Dir { get; set; }

        public float FullDistance { get; set; }

        public float MaxAngle { get; set; }

        public DirectionData BestDirection { get; set; }

        public int BestDirectionIndex { get; set; }

        public List<DirectionData> Directions { get; set; }
    }
}

