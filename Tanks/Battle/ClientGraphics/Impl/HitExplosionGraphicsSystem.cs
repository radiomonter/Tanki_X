namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HitExplosionGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateBlockedExplosionEffect(BaseShotEvent evt, BlockedWeaponNode node)
        {
            HitExplosionGraphicsComponent hitExplosionGraphics = node.hitExplosionGraphics;
            WeaponBlockedComponent weaponBlocked = node.weaponBlocked;
            Vector3 position = weaponBlocked.BlockPoint - (evt.ShotDirection * hitExplosionGraphics.ExplosionOffset);
            if (hitExplosionGraphics.UseForBlockedWeapon)
            {
                this.DrawExplosionEffect(position, weaponBlocked.BlockNormal, hitExplosionGraphics.ExplosionAsset, hitExplosionGraphics.ExplosionDuration, node);
            }
        }

        [OnEventFire]
        public void CreateExplosionEffect(ExplosionEvent evt, TankNode tank)
        {
            Vector3 position = tank.tankVisualRoot.transform.TransformPoint(evt.Target.LocalHitPoint) + evt.ExplosionOffset;
            this.DrawExplosionEffect(position, evt.ExplosionOffset, evt.Asset, evt.Duration, tank);
        }

        [OnEventFire]
        public void CreateExplosionOnEachTarget(HitEvent evt, WeaponNode weapon)
        {
            HitExplosionGraphicsComponent hitExplosionGraphics = weapon.hitExplosionGraphics;
            Vector3 fireDirectionWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetFireDirectionWorld();
            foreach (HitTarget target in evt.Targets)
            {
                ExplosionEvent eventInstance = new ExplosionEvent {
                    ExplosionOffset = -fireDirectionWorld * hitExplosionGraphics.ExplosionOffset,
                    HitDirection = target.HitDirection,
                    Asset = hitExplosionGraphics.ExplosionAsset,
                    Duration = hitExplosionGraphics.ExplosionDuration,
                    Target = target
                };
                base.ScheduleEvent(eventInstance, target.Entity);
            }
            if (evt.StaticHit != null)
            {
                Vector3 position = evt.StaticHit.Position - (fireDirectionWorld * hitExplosionGraphics.ExplosionOffset);
                this.DrawExplosionEffect(position, evt.StaticHit.Normal, hitExplosionGraphics.ExplosionAsset, hitExplosionGraphics.ExplosionDuration, weapon);
            }
        }

        private void DrawExplosionEffect(Vector3 position, Vector3 dir, GameObject asset, float duration, Node entity)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = asset,
                AutoRecycleTime = duration
            };
            base.ScheduleEvent(eventInstance, entity);
            Transform instance = eventInstance.Instance;
            instance.position = position;
            instance.rotation = Quaternion.Euler(dir);
            instance.gameObject.SetActive(true);
            instance.gameObject.GetComponent<ParticleSystem>().Play(true);
        }

        public class BlockedWeaponNode : Node
        {
            public WeaponComponent weapon;
            public HitExplosionGraphicsComponent hitExplosionGraphics;
            public WeaponBlockedComponent weaponBlocked;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankVisualRootComponent tankVisualRoot;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public HitExplosionGraphicsComponent hitExplosionGraphics;
            public MuzzlePointComponent muzzlePoint;
        }
    }
}

