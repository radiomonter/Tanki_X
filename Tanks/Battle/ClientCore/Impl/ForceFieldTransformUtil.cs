namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ForceFieldTransformUtil
    {
        public static float RAYCAST_TO_GROUND_MAX_DISTANCE = 3f;
        public static float DISTANCE_FROM_WEAPON = 7.3f;

        public static bool CanFallToTheGround(Transform weaponTransform)
        {
            RaycastHit hit;
            return HitToTheGround(weaponTransform, out hit);
        }

        private static Vector3 GetPositionInFrontOfWeapon(Transform weaponTransform) => 
            weaponTransform.position + (weaponTransform.TransformDirection(Vector3.forward) * DISTANCE_FROM_WEAPON);

        public static ForceFieldTranformComponent GetTransformComponent(Transform weaponTransform)
        {
            RaycastHit hit;
            Quaternion quaternion = Quaternion.Euler(0f, weaponTransform.rotation.eulerAngles.y, 0f);
            if (!HitToTheGround(weaponTransform, out hit))
            {
                ForceFieldTranformComponent component = new ForceFieldTranformComponent();
                Movement movement = new Movement {
                    Position = GetPositionInFrontOfWeapon(weaponTransform),
                    Orientation = quaternion
                };
                component.Movement = movement;
                return component;
            }
            Movement movement3 = new Movement {
                Position = hit.point,
                Orientation = Quaternion.FromToRotation(Vector3.up, hit.normal) * quaternion
            };
            return new ForceFieldTranformComponent { Movement = movement3 };
        }

        private static bool HitToTheGround(Transform weaponTransform, out RaycastHit hitInfo)
        {
            RaycastHit hit;
            Vector3 vector2 = weaponTransform.TransformDirection(Vector3.forward);
            Vector3 origin = weaponTransform.position + (vector2 * DISTANCE_FROM_WEAPON);
            bool hitExist = Physics.Raycast(origin, Vector3.down, out hitInfo, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            if (!hitExist)
            {
                vector2.y = 0f;
                origin = weaponTransform.position + (vector2 * DISTANCE_FROM_WEAPON);
                hitExist = Physics.Raycast(origin, Vector3.down, out hitInfo, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            }
            hitExist = ImproveHitWithLeftRightHit(hitExist, hitInfo, origin, out hit);
            return ImproveHitWithUpperHit(weaponTransform, hitExist, hit, out hitInfo);
        }

        private static bool ImproveHitWithLeftRightHit(bool hitExist, RaycastHit hitInfo, Vector3 position, out RaycastHit result)
        {
            RaycastHit hit;
            RaycastHit hit2;
            bool flag = Physics.Raycast(position + (Vector3.left * 2.5f), Vector3.down, out hit, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            bool flag2 = Physics.Raycast(position + (Vector3.right * 2.5f), Vector3.down, out hit2, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC);
            result = hitInfo;
            if (flag && flag2)
            {
                if (!hitExist)
                {
                    result = hit;
                    Vector3 vector = result.point;
                    vector.x = position.x;
                    vector.z = position.z;
                    result.point = vector;
                    return flag;
                }
                if (hitInfo.point.y.Equals(hit.point.y) || hitInfo.point.y.Equals(hit2.point.y))
                {
                    return hitExist;
                }
                if (!hit.point.y.Equals(hit2.point.y))
                {
                    return hitExist;
                }
                Vector3 point = result.point;
                point.y = hit.point.y;
                result.point = point;
            }
            return hitExist;
        }

        private static bool ImproveHitWithUpperHit(Transform weaponTransform, bool hitExist, RaycastHit hitInfo, out RaycastHit result)
        {
            RaycastHit hit;
            Vector3 position = (weaponTransform.position + (weaponTransform.TransformDirection(Vector3.forward) * DISTANCE_FROM_WEAPON)) + (Vector3.up * RAYCAST_TO_GROUND_MAX_DISTANCE);
            bool flag = ImproveHitWithLeftRightHit(Physics.Raycast(position, Vector3.down, out result, RAYCAST_TO_GROUND_MAX_DISTANCE, LayerMasks.STATIC), result, position, out hit);
            result = hitInfo;
            if (hitExist)
            {
                if (flag)
                {
                    result = (Vector3.Distance(position, hitInfo.point) >= Vector3.Distance(position, hit.point)) ? hit : hitInfo;
                }
                return true;
            }
            if (!flag)
            {
                return false;
            }
            result = hit;
            return flag;
        }
    }
}

