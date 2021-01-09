namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TankCollidersUnityComponent : MonoBehaviour, Component
    {
        public static readonly string BOUNDS_COLLIDER_NAME = "bounds";
        public static readonly string TANK_TO_STATIC_COLLIDER_NAME = "tank_to_static";
        public static readonly string TANK_TO_STATIC_TOP_COLLIDER_NAME = "top";
        public static readonly string TANK_TO_TANK_COLLIDER_NAME = "tank_to_tank";
        public static readonly string TARGETING_COLLIDER_NAME = "target";
        public static readonly string FRICTION_COLLIDERS_ROOT_NAME = "friction";
        public float a = 0.82f;
        public float inclineSubstraction = 0.1f;
        public PhysicMaterial lowFrictionMaterial;
        public PhysicMaterial highFrictionMaterial;

        private void Awake()
        {
            this.GetBoundsCollider().enabled = false;
        }

        private GameObject FindChildCollider(string childName)
        {
            foreach (Collider collider in base.transform.GetComponentsInChildren<Collider>(true))
            {
                if (collider.name.Equals(childName, StringComparison.OrdinalIgnoreCase))
                {
                    return collider.gameObject;
                }
            }
            throw new ColliderNotFoundException(this, childName);
        }

        public Vector3 GetBoundsCenterGlobal()
        {
            BoxCollider boundsCollider = this.GetBoundsCollider();
            return boundsCollider.transform.TransformPoint(boundsCollider.center);
        }

        public BoxCollider GetBoundsCollider() => 
            this.FindChildCollider(BOUNDS_COLLIDER_NAME).GetComponent<BoxCollider>();

        public Collider GetTankToStaticTopCollider() => 
            this.FindChildCollider(TANK_TO_STATIC_TOP_COLLIDER_NAME).GetComponent<Collider>();

        public Collider GetTankToTankCollider() => 
            this.FindChildCollider(TANK_TO_TANK_COLLIDER_NAME).GetComponent<Collider>();

        public Collider GetTargetingCollider() => 
            this.FindChildCollider(TARGETING_COLLIDER_NAME).GetComponent<Collider>();
    }
}

