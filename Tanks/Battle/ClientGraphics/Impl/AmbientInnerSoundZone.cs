namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class AmbientInnerSoundZone : AmbientSoundZone
    {
        [SerializeField]
        private Collider[] zoneColliders;
        private int collidersLength;

        public void FinalizeInnerZone()
        {
            for (int i = 0; i < this.collidersLength; i++)
            {
                Collider collider = this.zoneColliders[i];
                DestroyObject(collider.gameObject);
            }
        }

        public void InitInnerZone()
        {
            this.collidersLength = this.zoneColliders.Length;
            for (int i = 0; i < this.collidersLength; i++)
            {
                Collider collider = this.zoneColliders[i];
                collider.transform.SetParent(null, true);
            }
        }

        public bool IsActualZone(Transform listener)
        {
            for (int i = 0; i < this.collidersLength; i++)
            {
                Collider collider = this.zoneColliders[i];
                Bounds bounds = collider.bounds;
                if (bounds.Contains(listener.position))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

