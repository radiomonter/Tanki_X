namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class TargetFocusEffectSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Comparison<RaycastHit> <>f__am$cache0;

        [OnEventFire]
        public void AddTargetFocusComponent(NodeAddedEvent e, SelfTankNode tank, SingleNode<TargetFocusSettingsComponent> settings)
        {
            if (settings.component.Enabled)
            {
                tank.Entity.AddComponent<TargetFocusEffectComponent>();
                tank.Entity.AddComponent<TargetFocusVerticalTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusVerticalSectorTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusConicTargetingComponent>();
                tank.Entity.AddComponent<TargetFocusPelletConeComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusAngles(NodeAddedEvent e, TargetFocusEffectNode effect, [JoinByTank, Context] VerticalSectorTargetingNode weapon, [JoinByTank, Context] SelfTankNode tank)
        {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>())
            {
                weapon.verticalSectorsTargeting.VAngleDown += effect.targetFocusVerticalSectorTargeting.AdditionalAngleDown;
                weapon.verticalSectorsTargeting.VAngleUp += effect.targetFocusVerticalSectorTargeting.AdditionalAngleUp;
                weapon.verticalSectorsTargeting.HAngle += effect.targetFocusVerticalSectorTargeting.AdditionalAngleHorizontal;
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusAngles(NodeAddedEvent e, TargetFocusEffectNode effect, [JoinByTank, Context] VerticalTargetingNode weapon, [JoinByTank, Context] SelfTankNode tank)
        {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>())
            {
                float angleDown = weapon.verticalTargeting.AngleDown;
                float angleUp = weapon.verticalTargeting.AngleUp;
                float num3 = ((float) weapon.verticalTargeting.NumRaysUp) / angleUp;
                float num4 = ((float) weapon.verticalTargeting.NumRaysDown) / angleDown;
                weapon.verticalTargeting.AngleDown += effect.targetFocusVerticalTargeting.AdditionalAngleDown;
                weapon.verticalTargeting.AngleUp += effect.targetFocusVerticalTargeting.AdditionalAngleUp;
                weapon.verticalTargeting.NumRaysUp = Mathf.RoundToInt(num3 * weapon.verticalTargeting.AngleUp);
                weapon.verticalTargeting.NumRaysDown = Mathf.RoundToInt(num4 * weapon.verticalTargeting.AngleDown);
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void ApplyTargetFocusEffect(NodeAddedEvent e, SelfTankNode tank, [JoinByTank, Context] ConicTargetingNode weapon, [JoinByTank, Context] TargetFocusEffectNode effect)
        {
            if (!weapon.Entity.HasComponent<WeaponPreparedByTargetFocusEffectComponent>())
            {
                float halfConeAngle = weapon.conicTargeting.HalfConeAngle;
                float num2 = ((float) weapon.conicTargeting.HalfConeNumRays) / halfConeAngle;
                weapon.conicTargeting.HalfConeAngle += effect.targetFocusConicTargeting.AdditionalHalfConeAngle;
                weapon.conicTargeting.HalfConeNumRays = Mathf.RoundToInt(num2 * weapon.conicTargeting.HalfConeAngle);
                weapon.Entity.AddComponent<WeaponPreparedByTargetFocusEffectComponent>();
            }
        }

        [OnEventFire]
        public void CreateReticle(NodeAddedEvent e, ReticleResourceNode reticleNode, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode, [JoinAll] SelfTankNode selfTank, [JoinByUser] WeaponPartNode weapon)
        {
            reticleNode.reticle.Hammer = weapon.Entity.HasComponent<HammerComponent>();
            reticleNode.reticle.CanvasSize = canvasNode.component.screensLayer.rect.size;
            reticleNode.reticle.Create(reticleNode.resourceData.Data, canvasNode.component.transform);
            selfTank.tankGroup.Attach(reticleNode.Entity);
        }

        [OnEventFire]
        public void DeinitLaser(NodeRemoveEvent e, WeaponWithLaser weapon)
        {
            foreach (ShaftAimingLaserBehaviour behaviour in weapon.shaftAimingLaser.EffectInstances)
            {
                if (behaviour != null)
                {
                    behaviour.Kill();
                }
            }
            weapon.Entity.RemoveComponentIfPresent<ShaftAimingLaserReadyComponent>();
        }

        [OnEventFire]
        public void DeinitLaser(NodeRemoveEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponWithLaser weapon)
        {
            foreach (ShaftAimingLaserBehaviour behaviour in weapon.shaftAimingLaser.EffectInstances)
            {
                if (behaviour != null)
                {
                    behaviour.Kill();
                }
            }
            weapon.Entity.RemoveComponentIfPresent<ShaftAimingLaserReadyComponent>();
        }

        [OnEventFire]
        public void InitLaser(NodeAddedEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponWithLaser weapon, [JoinAll] SingleNode<LaserSightSettingsComponent> settings)
        {
            if (settings.component.Enabled)
            {
                GameObject asset = weapon.shaftAimingLaser.Asset;
                for (int i = 0; i < weapon.muzzlePoint.Points.Length; i++)
                {
                    GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                        Prefab = asset
                    };
                    base.ScheduleEvent(eventInstance, activeTank);
                    Transform instance = eventInstance.Instance;
                    GameObject gameObject = instance.gameObject;
                    ShaftAimingLaserBehaviour component = gameObject.GetComponent<ShaftAimingLaserBehaviour>();
                    weapon.shaftAimingLaser.EffectInstance = component;
                    CustomRenderQueue.SetQueue(gameObject, 0xc4e);
                    Transform transform = weapon.weaponInstance.WeaponInstance.transform;
                    instance.position = transform.position;
                    instance.rotation = transform.rotation;
                    gameObject.SetActive(true);
                    component.Init();
                    component.SetColor(Color.red);
                    component.Show();
                }
                weapon.Entity.AddComponent<ShaftAimingLaserReadyComponent>();
            }
        }

        [OnEventFire]
        public void OffReticle(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> activeTank, [JoinByTank] SingleNode<SelfTankComponent> self, [JoinAll] SingleNode<ReticleComponent> reticleNode)
        {
            reticleNode.component.Deactivate();
        }

        [OnEventFire]
        public void OnEnableEffect(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, WeaponWithReticleNode reticleTemplate, [JoinByTank, Context] TargetFocusEffectNode effect, [JoinByTank, Context] SelfTankNode selfTank, [JoinAll] Optional<SingleNode<ReticleComponent>> reticle)
        {
            if (!reticle.IsPresent())
            {
                base.CreateEntity(reticleTemplate.reticleTemplate.Template.TemplateId, reticleTemplate.reticleTemplate.ConfigPath);
            }
            else
            {
                reticle.Get().component.Reset();
            }
        }

        [OnEventFire]
        public void OnReticle(TankMovementInitEvent e, SingleNode<SelfTankComponent> tank, [JoinByTank] SingleNode<WeaponInstanceComponent> weaponInstance, [JoinByTank] SingleNode<SelfTankComponent> self, [JoinAll] SingleNode<ReticleComponent> reticleNode)
        {
            reticleNode.component.Reset();
        }

        [OnEventFire]
        public void RemoveReticle(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<ReticleComponent> reticleNode)
        {
            reticleNode.component.Destroy();
            base.DeleteEntity(reticleNode.Entity);
        }

        [OnEventFire]
        public void SetTeam(NodeAddedEvent e, ReticleResourceNode reticleNode, SelfWithTeam self, [JoinByUser] SingleNode<IsisComponent> isis)
        {
            reticleNode.reticle.CanHeal = true;
            reticleNode.reticle.TeamKey = self.teamGroup.Key;
        }

        private void UpdateLaser(TargetingData targeting, WeaponWithLaser weapon)
        {
            float maxLength = weapon.shaftAimingLaser.MaxLength;
            float minLength = weapon.shaftAimingLaser.MinLength;
            for (int i = 0; i < weapon.muzzlePoint.Points.Length; i++)
            {
                Vector3 forward = weapon.weaponInstance.WeaponInstance.transform.forward;
                Transform transform = weapon.muzzlePoint.Points[i];
                Vector3 localPosition = transform.localPosition;
                Vector3 vector2 = transform.position - (forward * localPosition.magnitude);
                ShaftAimingLaserBehaviour behaviour = weapon.shaftAimingLaser.EffectInstances[i];
                if (targeting.HasTargetHit() && ((targeting.BestDirection != null) && (targeting.BestDirection.Targets.Count > 0)))
                {
                    Vector3 hitPoint = targeting.BestDirection.Targets[0].HitPoint;
                    float num4 = Vector3.Distance(vector2, hitPoint);
                    behaviour.UpdateTargetPosition(vector2, hitPoint, num4 >= (minLength + transform.localPosition.magnitude), num4 > transform.localPosition.magnitude);
                }
                else
                {
                    bool flag;
                    bool flag2;
                    Vector3 point;
                    <UpdateLaser>c__AnonStorey0 storey = new <UpdateLaser>c__AnonStorey0 {
                        playerWeaponCollider = weapon.weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider
                    };
                    List<RaycastHit> list = Physics.RaycastAll(vector2, forward, maxLength, LayerMasks.VISUAL_TARGETING).ToList<RaycastHit>();
                    list.RemoveAll(new Predicate<RaycastHit>(storey.<>m__0));
                    if (list.Count <= 0)
                    {
                        flag2 = true;
                        flag = false;
                        point = vector2 + (forward * maxLength);
                    }
                    else
                    {
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = (a, b) => (a.distance >= b.distance) ? 1 : -1;
                        }
                        list.Sort(<>f__am$cache0);
                        point = list[0].point;
                        flag2 = Vector3.Distance(vector2, point) >= (minLength + transform.localPosition.magnitude);
                        flag = Vector3.Distance(vector2, point) > transform.localPosition.magnitude;
                    }
                    behaviour.UpdateTargetPosition(vector2, point, flag2, flag);
                    weapon.shaftAimingLaser.CurrentLaserDirection = forward;
                }
            }
        }

        [OnEventFire]
        public void UpdateReticle(UpdateEvent e, SelfActiveTankNode activeTank, [JoinByTank] WeaponNode weapon, [JoinByTank] Optional<WeaponWithLaser> weaponWithLaserNode, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode, [JoinAll] Optional<SingleNode<ReticleComponent>> reticleNode, [JoinAll] SingleNode<LaserSightSettingsComponent> settings)
        {
            ReticleComponent component = null;
            WeaponWithLaser laser = null;
            if (reticleNode.IsPresent())
            {
                component = reticleNode.Get().component;
            }
            if (weaponWithLaserNode.IsPresent() && settings.component.Enabled)
            {
                laser = weaponWithLaserNode.Get();
            }
            if ((component != null) && (weapon.Entity.HasComponent<ShaftComponent>() && (weapon.Entity.HasComponent<ShaftAimingWorkingStateComponent>() || (weapon.Entity.HasComponent<ShaftAimingWorkFinishStateComponent>() || weapon.Entity.HasComponent<ShaftAimingWorkActivationStateComponent>()))))
            {
                component.SetFree();
            }
            else if ((component != null) || (laser != null))
            {
                if (!weapon.Entity.HasComponent<WeaponUnblockedComponent>() && ((component != null) && (laser == null)))
                {
                    component.SetFree();
                }
                else
                {
                    TargetingData targetingData = BattleCache.targetingData.GetInstance().Init();
                    base.ScheduleEvent(BattleCache.targetingEvent.GetInstance().Init(targetingData), weapon);
                    if (component != null)
                    {
                        if (targetingData.HasTargetHit() && ((targetingData.BestDirection != null) && (targetingData.BestDirection.Targets.Count > 0)))
                        {
                            component.SetTargets(targetingData.BestDirection.Targets, canvasNode.component.screensLayer.rect.size);
                        }
                        else
                        {
                            component.SetFree();
                        }
                    }
                    if (laser != null)
                    {
                        this.UpdateLaser(targetingData, laser);
                    }
                }
            }
        }

        [Inject]
        public static BattleFlowInstancesCache BattleCache { get; set; }

        [CompilerGenerated]
        private sealed class <UpdateLaser>c__AnonStorey0
        {
            internal MeshCollider playerWeaponCollider;

            internal bool <>m__0(RaycastHit x) => 
                x.collider == this.playerWeaponCollider;
        }

        public class ConicTargetingNode : Node
        {
            public ConicTargetingComponent conicTargeting;
            public TankGroupComponent tankGroup;
        }

        public class ReticleResourceNode : Node
        {
            public ReticleComponent reticle;
            public ResourceDataComponent resourceData;
            public AssetReferenceComponent assetReference;
        }

        public class SelfActiveTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
        }

        public class SelfWithTeam : Node
        {
            public SelfTankComponent selfTank;
            public TeamGroupComponent teamGroup;
        }

        public class TargetFocusEffectNode : Node
        {
            public TargetFocusEffectComponent targetFocusEffect;
            public TargetFocusVerticalTargetingComponent targetFocusVerticalTargeting;
            public TargetFocusVerticalSectorTargetingComponent targetFocusVerticalSectorTargeting;
            public TargetFocusConicTargetingComponent targetFocusConicTargeting;
            public TargetFocusPelletConeComponent targetFocusPelletCone;
            public TankGroupComponent tankGroup;
        }

        public class VerticalSectorTargetingNode : Node
        {
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
            public TankGroupComponent tankGroup;
        }

        public class VerticalTargetingNode : Node
        {
            public VerticalTargetingComponent verticalTargeting;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
            public TankGroupComponent tankGroup;
        }

        public class WeaponPartNode : Node
        {
            public WeaponComponent weapon;
            public TankPartComponent tankPart;
        }

        [Not(typeof(ShaftComponent))]
        public class WeaponWithLaser : TargetFocusEffectSystem.WeaponNode
        {
            public WeaponVisualRootComponent weaponVisualRoot;
            public ShaftAimingLaserComponent shaftAimingLaser;
        }

        public class WeaponWithReticleNode : Node
        {
            public WeaponComponent weapon;
            public ReticleTemplateComponent reticleTemplate;
            public TankGroupComponent tankGroup;
        }
    }
}

