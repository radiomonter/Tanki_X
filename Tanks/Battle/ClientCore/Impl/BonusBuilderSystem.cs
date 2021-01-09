namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class BonusBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildBonusBox(NodeAddedEvent e, [Combine] BonusBoxBuildNode bonus, [Context, JoinByBattle] BonusClientConfigNode bonusClientConfig, [Context, JoinByMap] MapEffectNode mapEffect)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = (GameObject) bonus.resourceData.Data
            };
            base.ScheduleEvent(eventInstance, bonus);
            GameObject gameObject = eventInstance.Instance.gameObject;
            gameObject.AddComponent<BonusPhysicsBehaviour>().TriggerEntity = bonus.Entity;
            BonusBoxInstanceComponent component = new BonusBoxInstanceComponent {
                BonusBoxInstance = gameObject
            };
            bonus.Entity.AddComponent(component);
            gameObject.SetActive(true);
        }

        [OnEventFire]
        public void BuildParachuteIfNeed(NodeAddedEvent e, [Combine] BonusBoxDataNode bonus, ParachuteMapEffectNode mapEffect)
        {
            if (IsUnderCeil(bonus.position.Position) || this.IsOnGround(bonus.position.Position, bonus.bonusData, bonus.bonusDropTime))
            {
                bonus.Entity.AddComponent<BonusSpawnOnGroundStateComponent>();
                bonus.Entity.AddComponent<BonusGroundedStateComponent>();
                bonus.Entity.AddComponent<BonusSpawnStateComponent>();
            }
            else
            {
                GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                    Prefab = mapEffect.bonusParachuteMapEffect.Parachute
                };
                base.ScheduleEvent(eventInstance, bonus);
                Transform instance = eventInstance.Instance;
                GameObject gameObject = instance.gameObject;
                instance.parent = bonus.bonusBoxInstance.BonusBoxInstance.transform;
                instance.localPosition = new Vector3(0f, bonus.bonusData.BoxHeight, 0f);
                instance.rotation = Quaternion.identity;
                instance.localScale = Vector3.one;
                IEnumerator enumerator = instance.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Transform current = (Transform) enumerator.Current;
                        current.localRotation = Quaternion.identity;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                bonus.Entity.AddComponent(new BonusParachuteInstanceComponent(gameObject));
                gameObject.CollectComponentsInChildren(bonus.Entity);
                bonus.Entity.AddComponent<BonusSpawnStateComponent>();
                gameObject.SetActive(true);
            }
        }

        private static void CalculateGroundPointAndNormal(Vector3 spawnPosition, BonusDataComponent bonusData, BonusBoxInstantiatedNode bonus)
        {
            RaycastHit hit;
            if (!Physics.Raycast(spawnPosition, Vector3.down, out hit, float.PositiveInfinity, LayerMasks.STATIC))
            {
                bonusData.GroundPoint = spawnPosition;
                bonusData.GroundPointNormal = Vector3.up;
            }
            else
            {
                bonusData.GroundPoint = hit.point;
                bonusData.GroundPointNormal = hit.normal;
                bonus.rotation.RotationEuler = hit.transform.eulerAngles;
            }
        }

        private static void CalculateLandingPivot(Vector3 spawnPosition, ref BonusDataComponent bonusData)
        {
            RaycastHit hit;
            Vector3 normalized = Vector3.Cross(bonusData.GroundPointNormal, Vector3.up).normalized;
            Vector3 vector3 = Vector3.Cross(normalized, bonusData.GroundPointNormal);
            Vector3 vector4 = Vector3.Cross(normalized, Vector3.up);
            Vector3 origin = spawnPosition + ((vector4 * bonusData.BoxHeight) * 0.5f);
            bonusData.LandingPoint = bonusData.GroundPoint + (vector3 * ((bonusData.BoxHeight * 0.5f) / bonusData.GroundPointNormal.y));
            if (Physics.Raycast(origin, Vector3.down, out hit, float.PositiveInfinity, LayerMasks.STATIC) && ((bonusData.GroundPoint.y < hit.point.y) && (hit.point.y < bonusData.LandingPoint.y)))
            {
                bonusData.LandingPoint += vector3 * ((((bonusData.BoxHeight * 0.5f) / bonusData.GroundPointNormal.y) * (bonusData.LandingPoint.y - hit.point.y)) / (bonusData.LandingPoint.y - bonusData.GroundPoint.y));
            }
        }

        [OnEventFire]
        public void CalculateOnGroundPosition(NodeAddedEvent e, BonusOnGroundNode bonus)
        {
            RaycastHit hit;
            if (Physics.Raycast(bonus.position.Position, Vector3.down, out hit, float.PositiveInfinity, LayerMasks.STATIC))
            {
                Vector3 point = hit.point;
                bonus.position.Position = point;
                bonus.rotation.RotationEuler = hit.transform.eulerAngles;
                base.ScheduleEvent<SetBonusPositionEvent>(bonus);
            }
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, InstantiatedBonusNode bonus)
        {
            if (!bonus.bonusBoxInstance.Removed)
            {
                bonus.bonusBoxInstance.BonusBoxInstance.RecycleObject();
            }
        }

        private bool IsOnGround(Vector3 position, BonusDataComponent bonusData, BonusDropTimeComponent bonusDropTime) => 
            MathUtil.NearlyEqual(Date.Now.GetProgress(bonusDropTime.DropTime + bonusData.FallDuration, bonusData.AlignmentToGroundDuration), 1f, 0.01f);

        private static bool IsUnderCeil(Vector3 spawnPosition) => 
            Physics.Raycast(spawnPosition, Vector3.up, float.PositiveInfinity, LayerMasks.VISUAL_STATIC);

        [OnEventFire]
        public void PrepareBonusBoxData(NodeAddedEvent e, BonusBoxInstantiatedNode bonus)
        {
            BonusDataComponent bonusData = new BonusDataComponent();
            Vector3 position = bonus.position.Position;
            bonusData.BoxHeight = bonus.bonusBoxInstance.BonusBoxInstance.GetComponent<BoxCollider>().size.y;
            CalculateGroundPointAndNormal(position, bonusData, bonus);
            CalculateLandingPivot(position, ref bonusData);
            bonusData.FallDuration = (position.y - bonusData.LandingPoint.y) / bonus.bonusConfig.FallSpeed;
            if (bonusData.GroundPointNormal != Vector3.up)
            {
                bonusData.AlignmentToGroundDuration = (Mathf.Acos(bonusData.GroundPointNormal.y) * 57.29578f) / bonus.bonusConfig.AlignmentToGroundAngularSpeed;
                bonusData.LandingAxis = Vector3.Cross(Vector3.up, bonusData.GroundPointNormal);
            }
            bonus.Entity.AddComponent(bonusData);
        }

        [OnEventFire]
        public void PrepareParachuteData(NodeAddedEvent e, BonusWithParachuteNode bonus)
        {
            BonusDataComponent bonusData = bonus.bonusData;
            bonusData.ParachuteHalfHeight = bonus.bonusParachuteInstance.BonusParachuteInstance.GetComponentInChildren<Renderer>().bounds.size.y * 0.5f;
            bonusData.SwingPivotY = bonusData.BoxHeight + bonusData.ParachuteHalfHeight;
        }

        [OnEventFire]
        public void RequestBonusPrefab(NodeAddedEvent e, SingleNode<BonusBoxPrefabComponent> bonusPrefab)
        {
            bonusPrefab.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(bonusPrefab.component.AssetGuid)));
            bonusPrefab.Entity.AddComponent<AssetRequestComponent>();
        }

        public class BonusBoxBaseNode : Node
        {
            public BonusConfigComponent bonusConfig;
            public BonusComponent bonus;
            public BonusDropTimeComponent bonusDropTime;
            public PositionComponent position;
            public RotationComponent rotation;
            public BattleGroupComponent battleGroup;
        }

        public class BonusBoxBuildNode : BonusBuilderSystem.BonusBoxBaseNode
        {
            public ResourceDataComponent resourceData;
        }

        public class BonusBoxDataNode : BonusBuilderSystem.BonusBoxInstantiatedNode
        {
            public BonusDataComponent bonusData;
        }

        public class BonusBoxInstantiatedNode : BonusBuilderSystem.BonusBoxBuildNode
        {
            public BonusBoxInstanceComponent bonusBoxInstance;
        }

        public class BonusClientConfigNode : Node
        {
            public BonusClientConfigComponent bonusClientConfig;
            public BattleGroupComponent battleGroup;
            public MapGroupComponent mapGroup;
        }

        public class BonusOnGroundNode : BonusBuilderSystem.BonusBoxDataNode
        {
            public BonusSpawnOnGroundStateComponent bonusSpawnOnGroundState;
        }

        public class BonusWithParachuteNode : BonusBuilderSystem.BonusBoxDataNode
        {
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public TopParachuteMarkerComponent topParachuteMarker;
        }

        public class InstantiatedBonusNode : Node
        {
            public BonusComponent bonus;
            public BonusBoxInstanceComponent bonusBoxInstance;
        }

        public class MapEffectNode : Node
        {
            public MapEffectInstanceComponent mapEffectInstance;
            public MapGroupComponent mapGroup;
        }

        public class ParachuteMapEffectNode : BonusBuilderSystem.MapEffectNode
        {
            public BonusParachuteMapEffectComponent bonusParachuteMapEffect;
        }
    }
}

