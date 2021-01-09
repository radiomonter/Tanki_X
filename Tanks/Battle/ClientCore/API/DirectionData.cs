namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DirectionData
    {
        [CompilerGenerated]
        private static Func<TargetData, string> <>f__am$cache0;

        public DirectionData()
        {
            this.Targets = new List<TargetData>();
        }

        public void Clean()
        {
            this.Targets.Clear();
            this.StaticHit = null;
        }

        public float FirstAnyHitDistance()
        {
            if (this.StaticHit != null)
            {
                return (this.StaticHit.Position - this.Origin).magnitude;
            }
            if (!this.HasTargetHit())
            {
                throw new Exception("Havn't hit on direction");
            }
            return (this.Targets[0].HitPoint - this.Origin).magnitude;
        }

        public Vector3 FirstAnyHitNormal()
        {
            if (this.StaticHit != null)
            {
                return this.StaticHit.Normal;
            }
            if (!this.HasTargetHit())
            {
                throw new Exception("Havn't hit on direction");
            }
            return -this.Dir;
        }

        public Vector3 FirstAnyHitPosition()
        {
            if (this.StaticHit != null)
            {
                return this.StaticHit.Position;
            }
            if (!this.HasTargetHit())
            {
                throw new Exception("Havn't hit on direction");
            }
            return this.Targets[0].HitPoint;
        }

        public bool HasAnyHit() => 
            this.HasTargetHit() || this.HasStaticHit();

        public bool HasStaticHit() => 
            !ReferenceEquals(this.StaticHit, null);

        public bool HasTargetHit() => 
            this.Targets.Count > 0;

        public DirectionData Init() => 
            this.Init(Vector3.zero, Vector3.zero, 0f);

        public DirectionData Init(Vector3 origin, Vector3 dir, float angle)
        {
            this.Priority = 0f;
            this.Origin = origin;
            this.Dir = dir;
            this.Angle = angle;
            this.Extra = false;
            this.Targets.Clear();
            this.StaticHit = null;
            return this;
        }

        public override string ToString()
        {
            object[] objArray1 = new object[7];
            objArray1[0] = this.Priority;
            objArray1[1] = this.Origin;
            objArray1[2] = this.Dir;
            objArray1[3] = this.Angle;
            object[] objArray2 = objArray1;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = t => t.ToString();
            }
            objArray2[4] = string.Join(",", this.Targets.Select<TargetData, string>(<>f__am$cache0).ToArray<string>());
            object[] args = objArray2;
            args[5] = this.StaticHit;
            args[6] = this.Extra;
            return string.Format("Priority: {0}, Origin: {1}, Dir: {2}, Angle: {3}, Targets: {4}, StaticHit: {5}, Extra: {6}", args);
        }

        public float Priority { get; set; }

        public Vector3 Origin { get; set; }

        public Vector3 Dir { get; set; }

        public float Angle { get; set; }

        public bool Extra { get; set; }

        public List<TargetData> Targets { get; set; }

        public Tanks.Battle.ClientCore.API.StaticHit StaticHit { get; set; }
    }
}

