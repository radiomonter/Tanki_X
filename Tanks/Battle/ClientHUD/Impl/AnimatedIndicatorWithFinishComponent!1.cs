namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    [RequireComponent(typeof(Animator)), RequireComponent(typeof(NormalizedAnimatedValue))]
    public abstract class AnimatedIndicatorWithFinishComponent<T> : MonoBehaviour where T: Component, new()
    {
        private Entity screenEntity;
        private bool animationFinished;

        protected AnimatedIndicatorWithFinishComponent()
        {
        }

        private void CheckIfAnimationFinished(float currentVal = 1f, float targetVal = 1f)
        {
            if (!this.animationFinished && MathUtil.NearlyEqual(currentVal, targetVal, 0.005f))
            {
                this.SetAnimationFinished();
            }
        }

        private void OnEnable()
        {
            this.animationFinished = false;
        }

        private void SetAnimationFinished()
        {
            this.animationFinished = true;
            this.screenEntity.AddComponent<T>();
        }

        protected void SetEntity(Entity screenEntity)
        {
            this.screenEntity = screenEntity;
        }

        protected void TryToSetAnimationFinished()
        {
            if (!this.animationFinished)
            {
                this.SetAnimationFinished();
            }
        }

        protected void TryToSetAnimationFinished(float currentVal, float targetVal)
        {
            this.CheckIfAnimationFinished(currentVal, targetVal);
        }
    }
}

