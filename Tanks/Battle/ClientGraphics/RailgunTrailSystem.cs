namespace Tanks.Battle.ClientGraphics
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class RailgunTrailSystem : ECSSystem
    {
        private const int BIG_DISTANCE = 0x3e8;
        private const float TIPS_LENGTH = 2.5f;

        private void DrawShotTrailEffect(Vector3 shotPosition, Vector3 hitPosition, GameObject prefab, GameObject tipPrefab)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = tipPrefab,
                AutoRecycleTime = tipPrefab.GetComponent<LineRendererEffectBehaviour>().duration
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            GameObject gameObject = eventInstance.Instance.gameObject;
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
            LineRendererEffectBehaviour component = gameObject.GetComponent<LineRendererEffectBehaviour>();
            gameObject.SetActive(true);
            base.ScheduleEvent(eventInstance, new EntityStub());
            GameObject obj3 = eventInstance.Instance.gameObject;
            LineRendererEffectBehaviour behaviour2 = obj3.GetComponent<LineRendererEffectBehaviour>();
            obj3.SetActive(true);
            Vector3 vector = hitPosition - shotPosition;
            float magnitude = vector.magnitude;
            vector /= magnitude;
            if (magnitude <= 5f)
            {
                Vector3 vector5 = Vector3.Lerp(shotPosition, hitPosition, 0.5f);
                behaviour2.invertAlpha = true;
                Vector3[] vertices = new Vector3[] { shotPosition, vector5 };
                behaviour2.Init(component.LastScale, vertices);
                Vector3[] vectorArray5 = new Vector3[] { vector5, hitPosition };
                component.Init(behaviour2.LastScale, vectorArray5);
            }
            else
            {
                eventInstance.Prefab = prefab;
                eventInstance.AutoRecycleTime = prefab.GetComponent<LineRendererEffectBehaviour>().duration;
                base.ScheduleEvent(eventInstance, new EntityStub());
                GameObject obj4 = eventInstance.Instance.gameObject;
                LineRendererEffectBehaviour behaviour3 = obj4.GetComponent<LineRendererEffectBehaviour>();
                obj4.SetActive(true);
                Vector3 vector2 = vector * 2.5f;
                Vector3 vector3 = shotPosition + vector2;
                Vector3 vector4 = hitPosition - vector2;
                behaviour2.invertAlpha = true;
                Vector3[] vertices = new Vector3[] { shotPosition, vector3 };
                behaviour2.Init(component.LastScale, vertices);
                Vector3[] vectorArray2 = new Vector3[] { vector3, vector4 };
                behaviour3.Init(behaviour2.LastScale, vectorArray2);
                int index = 0;
                while (true)
                {
                    if (index >= behaviour2.LastScale.Length)
                    {
                        Vector3[] vectorArray3 = new Vector3[] { vector4, hitPosition };
                        component.Init(behaviour2.LastScale, vectorArray3);
                        break;
                    }
                    behaviour2.LastScale[index] += behaviour3.LastScale[index];
                    index++;
                }
            }
        }

        [OnEventFire]
        public void ShotTrail(BaseShotEvent evt, WeaponNode weapon)
        {
            RailgunTrailComponent railgunTrail = weapon.railgunTrail;
            Vector3 worldPosition = new MuzzleVisualAccessor(weapon.muzzlePoint).GetWorldPosition();
            Vector3 shotDirection = evt.ShotDirection;
            DirectionData data = weapon.targetCollector.Collect(worldPosition, shotDirection, 1000f, LayerMasks.VISUAL_STATIC);
            Vector3 hitPosition = !data.HasAnyHit() ? (worldPosition + (shotDirection * 1000f)) : data.FirstAnyHitPosition();
            this.DrawShotTrailEffect(worldPosition, hitPosition, railgunTrail.Prefab, railgunTrail.TipPrefab);
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public RailgunTrailComponent railgunTrail;
            public MuzzlePointComponent muzzlePoint;
            public WeaponUnblockedComponent weaponUnblocked;
            public TargetCollectorComponent targetCollector;
        }
    }
}

