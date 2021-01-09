namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TargetData
    {
        private bool validTarget = true;

        public TargetData Init()
        {
            this.Init(null, null);
            return this;
        }

        public TargetData Init(Entity targetEntity, Entity targetIncarnationEntity)
        {
            this.TargetEntity = targetEntity;
            this.TargetIncorantionEntity = targetIncarnationEntity;
            this.HitPoint = Vector3.zero;
            this.LocalHitPoint = Vector3.zero;
            this.TargetPosition = Vector3.zero;
            this.HitDirection = Vector3.zero;
            this.Priority = 0f;
            this.validTarget = true;
            return this;
        }

        public void SetTarget(Entity targetEntity, Entity targetIncarnationEntity)
        {
            this.TargetEntity = targetEntity;
            this.TargetIncorantionEntity = targetIncarnationEntity;
        }

        public override string ToString()
        {
            object[] args = new object[9];
            args[0] = this.TargetEntity;
            args[1] = this.HitPoint;
            args[2] = this.LocalHitPoint;
            args[3] = this.TargetPosition;
            args[4] = this.HitDirection;
            args[5] = this.HitDistance;
            args[6] = this.Priority;
            args[7] = this.ValidTarget;
            args[8] = this.PriorityWeakeningCount;
            return string.Format("Entity: {0}, HitPoint: {1}, LocalHitPoint: {2}, TargetPosition: {3}, HitDirection: {4}, HitDistance: {5}, Priority: {6}, ValidTarget: {7}, PriorityWeakeningCount: {8}", args);
        }

        public Entity TargetEntity { get; private set; }

        public Entity TargetIncorantionEntity { get; private set; }

        public Vector3 HitPoint { get; set; }

        public Vector3 LocalHitPoint { get; set; }

        public Vector3 TargetPosition { get; set; }

        public Vector3 HitDirection { get; set; }

        public float HitDistance { get; set; }

        public float Priority { get; set; }

        public bool ValidTarget
        {
            get => 
                this.validTarget;
            set => 
                this.validTarget = value;
        }

        public int PriorityWeakeningCount { get; set; }
    }
}

