namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShaftAimingReticleEffectSystem : ECSSystem
    {
        private const string RETICLE_SPOT_COLOR_PROPERTY = "_TintColor";

        [OnEventFire]
        public void FinalizeEffect(NodeRemoveEvent evt, ShaftAimingReadyReticleForNRNode nr, [JoinSelf] ShaftAimingReadyReticleNode weapon)
        {
            weapon.shaftAimingReticleEffect.ReticleSpotMaterialInstance = null;
            Object.Destroy(weapon.shaftAimingReticleEffect.InstanceReticle);
            weapon.Entity.RemoveComponent<ShaftAimingReticleReadyComponent>();
        }

        [OnEventFire]
        public void InitEffect(NodeAddedEvent evt, ShaftAimingReticleEffectNode weapon, [JoinAll] SingleNode<ScreensLayerComponent> canvasNode)
        {
            GameObject obj3 = Object.Instantiate<GameObject>(weapon.shaftAimingReticleEffect.AssetReticle);
            obj3.transform.SetParent(canvasNode.component.transform, false);
            obj3.transform.SetAsFirstSibling();
            weapon.shaftAimingReticleEffect.CanvasSize = canvasNode.component.selfRectTransform.rect.size;
            weapon.shaftAimingReticleEffect.InstanceReticle = obj3;
            weapon.shaftAimingReticleEffect.ReticleAnimator = obj3.GetComponent<Animator>();
            weapon.shaftAimingReticleEffect.DynamicReticle = obj3.transform.Find("reticle").transform;
            ShaftReticleSpotBehaviour behaviour = obj3.GetComponentsInChildren<ShaftReticleSpotBehaviour>(true)[0];
            behaviour.Init();
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour = behaviour;
            weapon.shaftAimingReticleEffect.ReticleGroup = obj3.GetComponent<CanvasGroup>();
            RawImage component = behaviour.gameObject.GetComponent<RawImage>();
            Material material = Object.Instantiate<Material>(component.material);
            weapon.shaftAimingReticleEffect.ReticleSpotMaterialInstance = material;
            component.material = material;
            component.material.SetColor("_TintColor", weapon.shaftAimingColorEffect.ChoosenColor);
            weapon.Entity.AddComponent<ShaftAimingReticleReadyComponent>();
        }

        [OnEventFire]
        public void LaunchEffect(NodeAddedEvent evt, ShaftAimingWorkActivationReticleEffectNode weapon)
        {
            ShaftAimingReticleEffectComponent shaftAimingReticleEffect = weapon.shaftAimingReticleEffect;
            this.UpdateImageAlpha(0f, shaftAimingReticleEffect);
            shaftAimingReticleEffect.InstanceReticle.SetActive(true);
            shaftAimingReticleEffect.ReticleAnimator.SetFloat("Time", weapon.shaftEnergy.UnloadAimingEnergyPerSec);
        }

        [OnEventFire]
        public void SetEffectOpaque(NodeAddedEvent evt, ShaftAimingWorkingReticleEffectNode weapon)
        {
            ShaftAimingReticleEffectComponent shaftAimingReticleEffect = weapon.shaftAimingReticleEffect;
            this.UpdateImageAlpha(1f, shaftAimingReticleEffect);
        }

        [OnEventFire]
        public void StopEffect(NodeAddedEvent evt, ShaftAimingIdleReticleEffectNode weapon)
        {
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour.SetDefaultSize();
            weapon.shaftAimingReticleEffect.InstanceReticle.SetActive(false);
        }

        [OnEventFire]
        public void UpdateEffectAlpha(UpdateEvent evt, ShaftAimingWorkActivationReticleEffectNode weapon)
        {
            float a = weapon.shaftAimingWorkActivationState.ActivationTimer / weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec;
            this.UpdateImageAlpha(a, weapon.shaftAimingReticleEffect);
            Vector3 barrelOriginWorld = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance).GetBarrelOriginWorld();
            float z = weapon.muzzlePoint.Current.transform.localPosition.z;
            Vector2 canvasSize = weapon.shaftAimingReticleEffect.CanvasSize;
            Vector2 vector5 = Camera.main.WorldToScreenPoint(barrelOriginWorld + (weapon.weaponInstance.WeaponInstance.transform.forward * z));
            Vector2 vector6 = new Vector2((vector5.x / ((float) Screen.width)) * canvasSize.x, (vector5.y / ((float) Screen.height)) * canvasSize.y);
            weapon.shaftAimingReticleEffect.DynamicReticle.localPosition = (Vector3) (vector6 - (canvasSize / 2f));
        }

        private void UpdateImageAlpha(float a, ShaftAimingReticleEffectComponent effect)
        {
            effect.ReticleGroup.alpha = a;
        }

        [OnEventFire]
        public void UpdateReticleSpotScale(WeaponRotationUpdateAimEvent e, ShaftAimingWorkingReticleEffectNode weapon)
        {
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            float distance = Vector3.Magnitude(weapon.shaftAimingTargetPointContainer.Point - accessor.GetBarrelOriginWorld());
            Vector3 barrelOriginWorld = accessor.GetBarrelOriginWorld();
            float z = weapon.muzzlePoint.Current.transform.localPosition.z;
            Vector2 canvasSize = weapon.shaftAimingReticleEffect.CanvasSize;
            Vector2 vector7 = Camera.main.WorldToScreenPoint(barrelOriginWorld + (weapon.shaftAimingWorkingState.WorkingDirection * z));
            Vector2 vector8 = new Vector2((vector7.x / ((float) Screen.width)) * canvasSize.x, (vector7.y / ((float) Screen.height)) * canvasSize.y);
            weapon.shaftAimingReticleEffect.DynamicReticle.localPosition = (Vector3) (vector8 - (canvasSize / 2f));
            weapon.shaftAimingReticleEffect.ShaftReticleSpotBehaviour.SetSizeByDistance(distance, weapon.shaftAimingTargetPointContainer.IsInsideTankPart);
        }

        public class ShaftAimingIdleReticleEffectNode : Node
        {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
        }

        public class ShaftAimingReadyReticleForNRNode : Node
        {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingReadyReticleNode : Node
        {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingReticleEffectNode : Node
        {
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;
            public ShaftAimingColorEffectPreparedComponent shaftAimingColorEffectPrepared;
        }

        public class ShaftAimingWorkActivationReticleEffectNode : Node
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
            public ShaftEnergyComponent shaftEnergy;
            public ShaftStateConfigComponent shaftStateConfig;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }

        public class ShaftAimingWorkingReticleEffectNode : Node
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingTargetPointContainerComponent shaftAimingTargetPointContainer;
            public ShaftAimingReticleEffectComponent shaftAimingReticleEffect;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftAimingReticleReadyComponent shaftAimingReticleReady;
            public MuzzlePointComponent muzzlePoint;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

