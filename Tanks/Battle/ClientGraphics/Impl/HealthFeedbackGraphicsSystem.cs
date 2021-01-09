namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HealthFeedbackGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void ChangeHealthVignette(HealthChangedEvent evt, SelfActiveTankNode tank, [JoinAll] ReadyBattleCameraNode camera)
        {
            float num3 = tank.health.CurrentHealth / tank.health.MaxHealth;
            camera.healthFeedbackCameraPrepared.TargetIntensity = 1f - Mathf.Clamp01(num3);
        }

        [OnEventFire]
        public void ChangeHealthVignette(UpdateEvent evt, SelfTankNode tank, [JoinAll] ReadyBattleCameraNode camera, [JoinAll] SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect)
        {
            float damageIntensity = camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity;
            float targetIntensity = camera.healthFeedbackCameraPrepared.TargetIntensity;
            float f = targetIntensity - damageIntensity;
            if (Mathf.Abs(f) > 0.0001f)
            {
                float num4 = Mathf.Sign(f) * mapEffect.component.IntensitySpeed;
                float num5 = damageIntensity + (num4 * evt.DeltaTime);
                num5 = (num4 <= 0f) ? Mathf.Clamp(num5, targetIntensity, damageIntensity) : Mathf.Clamp(num5, damageIntensity, targetIntensity);
                camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity = num5;
            }
        }

        private void DisableHealthVignette(ReadyBattleCameraNode camera)
        {
            camera.healthFeedbackCameraPrepared.TargetIntensity = 0f;
            camera.healthFeedbackCameraPrepared.HealthFeedbackPostEffect.DamageIntensity = 0f;
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeAddedEvent evt, SelfDeadTankNode tank, ReadyBattleCameraNode camera)
        {
            camera.healthFeedbackCameraPrepared.TargetIntensity = 0f;
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeAddedEvent evt, SelfTankNode tank, ReadyBattleCameraNode camera)
        {
            this.DisableHealthVignette(camera);
        }

        [OnEventFire]
        public void DisableHealthVignette(NodeRemoveEvent evt, SelfTankNode tank, [Context, JoinAll] ReadyBattleCameraNode camera)
        {
            this.DisableHealthVignette(camera);
        }

        [OnEventFire]
        public void InitBattleCameraForHealthFeedback(NodeAddedEvent e, BattleCameraNode camera, SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect, SingleNode<GameTankSettingsComponent> settings)
        {
            if (settings.component.HealthFeedbackEnabled)
            {
                HealthFeedbackPostEffect healthFeedbackPostEffect = camera.camera.UnityCamera.gameObject.AddComponent<HealthFeedbackPostEffect>();
                healthFeedbackPostEffect.Init(mapEffect.component.SourceMaterial);
                camera.Entity.AddComponent(new HealthFeedbackCameraPreparedComponent(healthFeedbackPostEffect));
            }
        }

        public class BattleCameraNode : Node
        {
            public CameraComponent camera;
            public BattleCameraComponent battleCamera;
        }

        public class ReadyBattleCameraNode : HealthFeedbackGraphicsSystem.BattleCameraNode
        {
            public HealthFeedbackCameraPreparedComponent healthFeedbackCameraPrepared;
        }

        public class SelfActiveTankNode : HealthFeedbackGraphicsSystem.SelfTankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class SelfDeadTankNode : HealthFeedbackGraphicsSystem.SelfTankNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public HealthComponent health;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }
    }
}

