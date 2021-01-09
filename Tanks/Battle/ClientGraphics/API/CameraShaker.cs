namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class CameraShaker : MonoBehaviour
    {
        private const float CAMERA_COLLISION_OFFSET = 0.5f;
        [SerializeField]
        private Vector3 defaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);
        [SerializeField]
        private Vector3 defaultRotInfluence = new Vector3(1f, 1f, 1f);
        private Vector3 posAddShake;
        private Vector3 rotAddShake;
        private List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();
        private Cache<CameraShakeInstance> shakerInstancesCache = new CacheImpl<CameraShakeInstance>();

        private Vector3 CalculateLocalSpaceCameraPosition(Vector3 posAddShake)
        {
            RaycastHit hit;
            if (this.cameraShakeInstances.Count == 0)
            {
                return Vector3.zero;
            }
            Vector3 origin = base.transform.parent.localToWorldMatrix.MultiplyPoint(Vector3.zero);
            Vector3 vector3 = base.transform.parent.localToWorldMatrix.MultiplyPoint(posAddShake) - origin;
            float magnitude = vector3.magnitude;
            Vector3 normalized = vector3.normalized;
            if (normalized.sqrMagnitude < 0.5f)
            {
                return Vector3.zero;
            }
            if (!Physics.Raycast(new Ray(origin, normalized), out hit, magnitude + 0.5f, LayerMasks.STATIC))
            {
                return posAddShake;
            }
            Vector3 point = hit.point;
            Vector3 vector6 = point - origin;
            return base.transform.parent.worldToLocalMatrix.MultiplyPoint(point - (vector6.normalized * Mathf.Min(0.5f, vector6.magnitude)));
        }

        private static unsafe Vector3 MultiplyVectors(Vector3 v, Vector3 w)
        {
            Vector3* vectorPtr1 = &v;
            vectorPtr1->x *= w.x;
            Vector3* vectorPtr2 = &v;
            vectorPtr2->y *= w.y;
            Vector3* vectorPtr3 = &v;
            vectorPtr3->z *= w.z;
            return v;
        }

        public CameraShakeInstance Shake(CameraShakeInstance shake)
        {
            this.cameraShakeInstances.Add(shake);
            return shake;
        }

        public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
        {
            CameraShakeInstance item = this.shakerInstancesCache.GetInstance().Init(magnitude, roughness, fadeInTime, fadeOutTime);
            item.positionInfluence = this.defaultPosInfluence;
            item.rotationInfluence = this.defaultRotInfluence;
            this.cameraShakeInstances.Add(item);
            return item;
        }

        public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
        {
            CameraShakeInstance item = this.shakerInstancesCache.GetInstance().Init(magnitude, roughness, fadeInTime, fadeOutTime);
            item.positionInfluence = posInfluence;
            item.rotationInfluence = rotInfluence;
            this.cameraShakeInstances.Add(item);
            return item;
        }

        private static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime)
        {
            Vector3 vector;
            vector.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
            vector.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);
            vector.z = Mathf.SmoothDampAngle(current.z, target.z, ref velocity.z, smoothTime);
            return vector;
        }

        public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
        {
            CameraShakeInstance item = this.shakerInstancesCache.GetInstance().Init(magnitude, roughness);
            item.positionInfluence = this.defaultPosInfluence;
            item.rotationInfluence = this.defaultRotInfluence;
            item.StartFadeIn(fadeInTime);
            this.cameraShakeInstances.Add(item);
            return item;
        }

        public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
        {
            CameraShakeInstance item = this.shakerInstancesCache.GetInstance().Init(magnitude, roughness);
            item.positionInfluence = posInfluence;
            item.rotationInfluence = rotInfluence;
            item.StartFadeIn(fadeInTime);
            this.cameraShakeInstances.Add(item);
            return item;
        }

        private void Update()
        {
            this.posAddShake = Vector3.zero;
            this.rotAddShake = Vector3.zero;
            for (int i = 0; (i < this.cameraShakeInstances.Count) && (i < this.cameraShakeInstances.Count); i++)
            {
                CameraShakeInstance item = this.cameraShakeInstances[i];
                if ((item.CurrentState == CameraShakeState.Inactive) && item.deleteOnInactive)
                {
                    this.cameraShakeInstances.RemoveAt(i);
                    i--;
                    this.shakerInstancesCache.Free(item);
                }
                else if (item.CurrentState != CameraShakeState.Inactive)
                {
                    this.posAddShake += MultiplyVectors(item.UpdateShake(), item.positionInfluence);
                    this.rotAddShake += MultiplyVectors(item.UpdateShake(), item.rotationInfluence);
                }
            }
            base.transform.SetLocalPositionSafe(this.CalculateLocalSpaceCameraPosition(this.posAddShake));
            base.transform.SetLocalEulerAnglesSafe(this.rotAddShake);
        }

        public List<CameraShakeInstance> ShakeInstances =>
            new List<CameraShakeInstance>(this.cameraShakeInstances);
    }
}

