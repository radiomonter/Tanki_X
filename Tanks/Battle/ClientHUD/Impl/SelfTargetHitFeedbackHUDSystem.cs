namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class SelfTargetHitFeedbackHUDSystem : ECSSystem
    {
        private const float VISIBILITY_RANGE = 50f;
        private const float EFFECT_INTERVAL = 0.5f;
        private const float SIN_45_DEGREE = 0.7071068f;
        private const bool MODIFY_HIT_DIRECTION = true;
        private const string EFFECT_INSTANCE_NAME = "SelfTargetHitHUDEffectInstance";

        private static bool CheckPossibilityForEffectInstancing(ReadyWeaponNode weapon) => 
            ((Time.time - weapon.weaponSelfTargetHitFeedbackTimer.LastTime) >= 0.5f) ? !weapon.cameraVisibleTrigger.IsVisibleAtRange(50f) : false;

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, NotStreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank, [JoinByTank] NotShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, false);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, NotStreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank, [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, StreamHitWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank, [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        [OnEventFire]
        public void CheckSelfTargetHit(DamageInfoTargetEvent e, StreamWeaponNode enemyWeapon, [JoinByUser] RemoteTankNode remoteTank, SelfTankNode selfTank, [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.CreateSelfTargetHitHUDFeedback(enemyWeapon, remoteTank, selfTank, camera, canvasNode, true);
        }

        private void CreateSelfTargetHitHUDFeedback(ReadyWeaponNode enemyWeapon, RemoteTankNode remoteTank, SelfTankNode selfTank, BattleCameraNode camera, SingleNode<ScreensLayerComponent> canvasNode, bool isShaft)
        {
            if (CheckPossibilityForEffectInstancing(enemyWeapon))
            {
                SelfTargetHitEffectHUDData? nullable = this.GetDataForSelfTargetHitEffect(enemyWeapon, selfTank, camera, canvasNode, isShaft);
                if (nullable != null)
                {
                    enemyWeapon.weaponSelfTargetHitFeedbackTimer.LastTime = Time.time;
                    SelfTargetHitFeedbackHUDInstanceComponent component = Object.Instantiate<SelfTargetHitFeedbackHUDInstanceComponent>(selfTank.selfTargetHitFeedbackHUDConfig.EffectPrefab, canvasNode.component.transform);
                    Entity entity = base.CreateEntity("SelfTargetHitHUDEffectInstance");
                    remoteTank.tankGroup.Attach(entity);
                    enemyWeapon.weaponSelfTargetHitFeedbackGroup.Attach(entity);
                    component.Init(entity, nullable.Value);
                }
            }
        }

        [OnEventFire]
        public void DetachHUDEffectInstance(NodeRemoveEvent e, ReadyWeaponNode enemyWeapon, [JoinBy(typeof(WeaponSelfTargetHitFeedbackGroupComponent)), Combine] SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> effect)
        {
            enemyWeapon.weaponSelfTargetHitFeedbackGroup.Detach(effect.Entity);
        }

        [OnEventFire]
        public void DetachHUDEffectInstance(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank, Combine] SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> effect)
        {
            tankIncarnation.tankGroup.Detach(effect.Entity);
        }

        private SelfTargetHitEffectHUDData? GetDataForSelfTargetHitEffect(ReadyWeaponNode enemyWeapon, SelfTankNode selfTank, BattleCameraNode camera, SingleNode<ScreensLayerComponent> canvasNode, bool isShaft) => 
            this.GetDataForSelfTargetHitEffect(enemyWeapon.weaponVisualRoot.transform.position, selfTank, camera, canvasNode, isShaft);

        private SelfTargetHitEffectHUDData? GetDataForSelfTargetHitEffect(Vector3 enemyWeaponWorldPosition, SelfTankNode selfTank, BattleCameraNode camera, SingleNode<ScreensLayerComponent> canvasNode, bool isShaft)
        {
            bool flag;
            Vector2 viewportPos = new Vector2(0.5f, 0.5f);
            Vector2 hitVecViewportSpace = viewportPos - WorldToViewportPointProjected(camera.camera.UnityCamera, enemyWeaponWorldPosition, out flag);
            if (isShaft)
            {
                hitVecViewportSpace.y = !flag ? -Mathf.Abs(hitVecViewportSpace.y) : Mathf.Abs(hitVecViewportSpace.y);
                Vector2 vector = viewportPos - hitVecViewportSpace;
            }
            Vector2? boundPosition = selfTank.selfTargetHitFeedbackHUDConfig.GetBoundPosition((Vector3) viewportPos, hitVecViewportSpace);
            if (boundPosition == null)
            {
                return null;
            }
            Vector2 vector4 = boundPosition.Value;
            Vector2 localPositionForCanvasByViewport = GetLocalPositionForCanvasByViewport(vector4, canvasNode);
            Vector2 vector6 = GetLocalPositionForCanvasByViewport(viewportPos, canvasNode) - localPositionForCanvasByViewport;
            Vector3 vector7 = new Vector3(vector6.x, vector6.y, 0f);
            float magnitude = vector7.magnitude;
            Vector3 vector9 = new Vector3(vector6.x, vector6.y, 0f);
            return new SelfTargetHitEffectHUDData(enemyWeaponWorldPosition, vector4, localPositionForCanvasByViewport, vector9.normalized, canvasNode.component.selfRectTransform.sizeDelta, magnitude);
        }

        private static Vector2 GetLocalPositionForCanvasByViewport(Vector2 viewportPos, SingleNode<ScreensLayerComponent> canvasNode)
        {
            RectTransform selfRectTransform = canvasNode.component.selfRectTransform;
            Vector2 vector = new Vector2(selfRectTransform.sizeDelta.x / 2f, selfRectTransform.sizeDelta.y / 2f);
            Vector2 vector4 = new Vector2(viewportPos.x * selfRectTransform.sizeDelta.x, viewportPos.y * selfRectTransform.sizeDelta.y);
            return (vector4 - vector);
        }

        [OnEventFire]
        public void InitEnemyWeapon(NodeAddedEvent e, [Combine] WeaponNode weapon, [JoinByUser, Context] RemoteTankNode remoteTank, SingleNode<GameTankSettingsComponent> settings)
        {
            if (settings.component.SelfTargetHitFeedbackEnabled)
            {
                weapon.Entity.AddComponent(new WeaponSelfTargetHitFeedbackTimerComponent(Time.time - 0.5f));
                weapon.Entity.CreateGroup<WeaponSelfTargetHitFeedbackGroupComponent>();
            }
        }

        [OnEventFire]
        public void RemoveEffect(NodeRemoveEvent e, SingleNode<SelfTargetHitFeedbackHUDInstanceComponent> node)
        {
            Object.DestroyObject(node.component.gameObject);
        }

        private void UpdateSelfTargetHitEffect(EffectInstanceFullNode effect, SelfTankNode selfTank, BattleCameraNode camera, SingleNode<ScreensLayerComponent> canvasNode, bool isShaftAiming)
        {
            SelfTargetHitEffectHUDData? nullable = this.GetDataForSelfTargetHitEffect(effect.selfTargetHitFeedbackHUDInstance.InitialData.EnemyWeaponWorldSpace, selfTank, camera, canvasNode, isShaftAiming);
            if (nullable != null)
            {
                effect.selfTargetHitFeedbackHUDInstance.UpdateTransform(nullable.Value);
            }
        }

        [OnEventFire]
        public void UpdateSelfTargetHitEffect(UpdateEvent e, EffectInstanceFullNode effect, [JoinAll] SelfTankNode selfTank, [JoinByTank] NotShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.UpdateSelfTargetHitEffect(effect, selfTank, camera, canvasNode, false);
        }

        [OnEventFire]
        public void UpdateSelfTargetHitEffect(UpdateEvent e, EffectInstanceFullNode effect, [JoinAll] SelfTankNode selfTank, [JoinByTank] ShaftAimingWeaponNode selfWeapon, [JoinAll] BattleCameraNode camera, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            this.UpdateSelfTargetHitEffect(effect, selfTank, camera, canvasNode, true);
        }

        private static Vector2 WorldToViewportPointProjected(Camera camera, Vector3 worldPos, out bool behindCameraForwardPlane)
        {
            Vector3 forward = camera.transform.forward;
            Vector3 rhs = worldPos - camera.transform.position;
            float num = Vector3.Dot(forward, rhs);
            behindCameraForwardPlane = num == 0f;
            if (behindCameraForwardPlane)
            {
                worldPos = camera.transform.position + (rhs - ((forward * num) * 1.01f));
            }
            return camera.WorldToViewportPoint(worldPos);
        }

        public class BattleCameraNode : Node
        {
            public CameraComponent camera;
            public BattleCameraComponent battleCamera;
        }

        public class EffectInstanceFullNode : Node
        {
            public SelfTargetHitFeedbackHUDInstanceComponent selfTargetHitFeedbackHUDInstance;
            public TankGroupComponent tankGroup;
            public SelfTargetHitFeedbackHUDSystem.WeaponSelfTargetHitFeedbackGroupComponent weaponSelfTargetHitFeedbackGroup;
        }

        [Not(typeof(ShaftWaitingStateComponent)), Not(typeof(ShaftAimingWorkActivationStateComponent)), Not(typeof(ShaftAimingWorkingStateComponent)), Not(typeof(ShaftAimingWorkFinishStateComponent))]
        public class NotShaftAimingWeaponNode : SelfTargetHitFeedbackHUDSystem.WeaponNode
        {
        }

        [Not(typeof(StreamWeaponComponent)), Not(typeof(DroneWeaponComponent))]
        public class NotStreamWeaponNode : SelfTargetHitFeedbackHUDSystem.ReadyWeaponNode
        {
        }

        public class ReadyWeaponNode : SelfTargetHitFeedbackHUDSystem.WeaponNode
        {
            public SelfTargetHitFeedbackHUDSystem.WeaponSelfTargetHitFeedbackTimerComponent weaponSelfTargetHitFeedbackTimer;
            public SelfTargetHitFeedbackHUDSystem.WeaponSelfTargetHitFeedbackGroupComponent weaponSelfTargetHitFeedbackGroup;
        }

        public class RemoteTankNode : SelfTargetHitFeedbackHUDSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : SelfTargetHitFeedbackHUDSystem.TankNode
        {
            public SelfTankComponent selfTank;
            public SelfTargetHitFeedbackHUDConfigComponent selfTargetHitFeedbackHUDConfig;
        }

        public class ShaftAimingWeaponNode : SelfTargetHitFeedbackHUDSystem.WeaponNode
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }

        public class StreamHitWeaponNode : SelfTargetHitFeedbackHUDSystem.ReadyWeaponNode
        {
            public StreamHitConfigComponent streamHitConfig;
        }

        [Not(typeof(StreamHitConfigComponent))]
        public class StreamWeaponNode : SelfTargetHitFeedbackHUDSystem.ReadyWeaponNode
        {
            public StreamWeaponComponent streamWeapon;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankVisualRootComponent tankVisualRoot;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankGroupComponent tankGroup;
            public UserGroupComponent userGroup;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public CameraVisibleTriggerComponent cameraVisibleTrigger;
            public WeaponVisualRootComponent weaponVisualRoot;
            public UserGroupComponent userGroup;
            public MuzzlePointComponent muzzlePoint;
        }

        public class WeaponSelfTargetHitFeedbackGroupComponent : GroupComponent
        {
            public WeaponSelfTargetHitFeedbackGroupComponent(Entity keyEntity) : base(keyEntity)
            {
            }

            public WeaponSelfTargetHitFeedbackGroupComponent(long key) : base(key)
            {
            }
        }

        public class WeaponSelfTargetHitFeedbackTimerComponent : Component
        {
            public WeaponSelfTargetHitFeedbackTimerComponent(float lastTime)
            {
                this.LastTime = lastTime;
            }

            public float LastTime { get; set; }
        }
    }
}

