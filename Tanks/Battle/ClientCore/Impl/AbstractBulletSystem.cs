namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public abstract class AbstractBulletSystem : ECSSystem
    {
        protected AbstractBulletSystem()
        {
        }

        protected void DestroyBullet(Entity bulletEntity)
        {
            bulletEntity.RemoveComponent<BulletComponent>();
            base.DeleteEntity(bulletEntity);
        }

        protected bool DestroyOnAnyTargetHit(Entity bulletEntity, BulletComponent bullet, BulletConfigComponent config, TargetingData targeting)
        {
            bool flag;
            using (List<DirectionData>.Enumerator enumerator = targeting.Directions.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        DirectionData current = enumerator.Current;
                        if (current.Targets.Count <= 0)
                        {
                            continue;
                        }
                        TargetData data2 = current.Targets.First<TargetData>();
                        this.SetPositionNearHitPoint(bullet, data2.HitPoint);
                        this.SendBulletTargetHitEvent(bulletEntity, bullet, data2.TargetEntity);
                        this.DestroyBullet(bulletEntity);
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        protected void InitBullet(BulletComponent bullet, Vector3 position, Vector3 direction, float radius, Rigidbody tankRigidbody)
        {
            position += direction * radius;
            position += ((direction * Vector3.Dot(direction, tankRigidbody.velocity.normalized)) * tankRigidbody.velocity.magnitude) * Time.smoothDeltaTime;
            bullet.LastUpdateTime = Time.time;
            bullet.Position = position;
            bullet.Direction = direction;
        }

        protected void MoveBullet(Entity bulletEntity, BulletComponent bullet)
        {
            float num = Time.time - bullet.LastUpdateTime;
            float num2 = bullet.Speed * num;
            bullet.Position += num2 * bullet.Direction;
            bullet.Distance += num2;
            bullet.LastUpdateTime = Time.time;
        }

        protected void PrepareTargetData(TargetData targetData, BulletComponent bulletComponent)
        {
            targetData.HitDistance += bulletComponent.Distance;
            targetData.HitDirection = bulletComponent.Direction;
        }

        protected void SendBulletStaticHitEvent(Entity bulletEntity, BulletComponent bullet)
        {
            BulletStaticHitEvent eventInstance = new BulletStaticHitEvent {
                Position = bullet.Position
            };
            base.ScheduleEvent(eventInstance, bulletEntity);
        }

        protected void SendBulletTargetHitEvent(Entity bulletEntity, BulletComponent bullet, Entity target)
        {
            BulletTargetHitEvent eventInstance = new BulletTargetHitEvent {
                Position = bullet.Position,
                Target = target
            };
            base.ScheduleEvent(eventInstance, bulletEntity);
        }

        protected void SetPositionNearHitPoint(BulletComponent bullet, Vector3 hitPoint)
        {
            Vector3 vector = hitPoint - bullet.Position;
            float num = Vector3.Dot(bullet.Direction, vector.normalized);
            bullet.Position += bullet.Direction * ((vector.magnitude * num) - bullet.Radius);
        }
    }
}

