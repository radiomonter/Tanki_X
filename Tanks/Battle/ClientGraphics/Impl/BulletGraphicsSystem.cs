namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class BulletGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void Build(NodeAddedEvent e, BulletNode node, [JoinBy(typeof(TankGroupComponent))] WeaponNode weaponNode)
        {
            BulletComponent bullet = node.bullet;
            Quaternion quaternion = Quaternion.LookRotation(bullet.Direction);
            BulletEffectInstanceComponent component = new BulletEffectInstanceComponent();
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = weaponNode.bulletEffect.BulletPrefab
            };
            base.ScheduleEvent(eventInstance, node);
            GameObject gameObject = eventInstance.Instance.gameObject;
            gameObject.transform.position = bullet.Position;
            gameObject.transform.rotation = quaternion;
            gameObject.SetActive(true);
            component.Effect = gameObject;
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
            node.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void Explosion(BulletStaticHitEvent e, Node node, [JoinByTank] WeaponNode weaponNode)
        {
            this.InstantiateExplosion(e, weaponNode);
        }

        [OnEventFire]
        public void Explosion(BulletTargetHitEvent e, Node node, [JoinByTank] WeaponNode weaponNode)
        {
            this.InstantiateExplosion(e, weaponNode);
        }

        private void InstantiateExplosion(BulletHitEvent e, WeaponNode weaponNode)
        {
            BulletEffectComponent bulletEffect = weaponNode.bulletEffect;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = bulletEffect.ExplosionPrefab,
                AutoRecycleTime = bulletEffect.ExplosionTime
            };
            base.ScheduleEvent(eventInstance, weaponNode);
            GameObject gameObject = eventInstance.Instance.gameObject;
            gameObject.transform.position = e.Position;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
        }

        [OnEventFire]
        public void Move(UpdateEvent e, BulletEffectNode node)
        {
            GameObject effect = node.bulletEffectInstance.Effect;
            if (effect)
            {
                BulletComponent bullet = node.bullet;
                effect.transform.position = bullet.Position;
                effect.transform.rotation = Quaternion.LookRotation(bullet.Direction);
            }
        }

        [OnEventFire]
        public void Remove(NodeRemoveEvent e, BulletEffectNode bulletNode)
        {
            bulletNode.bulletEffectInstance.Effect.RecycleObject();
        }

        public class BulletEffectNode : Node
        {
            public BulletComponent bullet;
            public BulletConfigComponent bulletConfig;
            public BulletEffectInstanceComponent bulletEffectInstance;
        }

        public class BulletNode : Node
        {
            public TankGroupComponent tankGroup;
            public BulletComponent bullet;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public BulletEffectComponent bulletEffect;
        }
    }
}

