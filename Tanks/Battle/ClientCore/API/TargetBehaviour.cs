namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TargetBehaviour : MonoBehaviour
    {
        public virtual bool AcceptAsTarget(Entity targetingOwner) => 
            true;

        public virtual bool CanSkip(Entity targetingOwner) => 
            false;

        public void Init(Entity targetEntity, Entity targetIcarnationEntity = null)
        {
            this.TargetEntity = targetEntity;
            Entity entity1 = targetIcarnationEntity;
            if (targetIcarnationEntity == null)
            {
                Entity local1 = targetIcarnationEntity;
                entity1 = targetEntity;
            }
            this.TargetIcarnationEntity = entity1;
        }

        public Entity TargetEntity { get; protected set; }

        public Entity TargetIcarnationEntity { get; protected set; }
    }
}

