namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class VulcanTurbineAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string turbineStateName = "TurbineRotation";
        [SerializeField]
        private int turbineLayerIndex;
        [SerializeField]
        private string turbineCoeffName = "turbineCoeff";
        [SerializeField]
        private string turbineStopName = "stopTurbine";
        [SerializeField]
        private int turbineMaxSpeedCoeff = 1;
        private int turbineStateID;
        private int turbineStopID;
        private int turbineCoeffID;
        private Animator vulcanAnimator;
        private float currentNrmTime;
        private float speedUpAcceleration;
        private float slowDownAcceleration;
        private float currentAcceleration;
        private float currentSpeed;
        private bool isRunningTurbine;

        private void Awake()
        {
            base.enabled = false;
        }

        public void Init(Animator animator, float speedUpTime, float slowDownTime)
        {
            this.vulcanAnimator = animator;
            this.turbineStateID = Animator.StringToHash(this.turbineStateName);
            this.turbineCoeffID = Animator.StringToHash(this.turbineCoeffName);
            this.turbineStopID = Animator.StringToHash(this.turbineStopName);
            this.speedUpAcceleration = ((float) this.turbineMaxSpeedCoeff) / speedUpTime;
            this.slowDownAcceleration = ((float) -this.turbineMaxSpeedCoeff) / slowDownTime;
            this.currentNrmTime = 0f;
            this.currentSpeed = 0f;
            this.vulcanAnimator.SetFloat(this.turbineCoeffID, this.currentSpeed);
            this.isRunningTurbine = false;
        }

        private void PlayAnimatorIfNeed()
        {
            if (!this.isRunningTurbine)
            {
                this.vulcanAnimator.Play(this.turbineStateID, this.turbineLayerIndex, this.currentNrmTime);
            }
        }

        private void StartChangeSpeedPhase(bool isSpeedUp)
        {
            this.vulcanAnimator.ResetTrigger(this.turbineStopID);
            this.currentAcceleration = !isSpeedUp ? this.slowDownAcceleration : this.speedUpAcceleration;
            this.PlayAnimatorIfNeed();
            base.enabled = true;
            this.isRunningTurbine = true;
        }

        public void StartShooting()
        {
            this.PlayAnimatorIfNeed();
            this.currentSpeed = this.turbineMaxSpeedCoeff;
            this.vulcanAnimator.SetFloat(this.turbineCoeffID, this.currentSpeed);
            base.enabled = false;
            this.isRunningTurbine = true;
        }

        public void StartSlowDown()
        {
            this.StartChangeSpeedPhase(false);
        }

        public void StartSpeedUp()
        {
            this.StartChangeSpeedPhase(true);
        }

        public void StopTurbine()
        {
            this.currentSpeed = 0f;
            this.currentNrmTime = Mathf.Repeat(this.vulcanAnimator.GetCurrentAnimatorStateInfo(this.turbineLayerIndex).normalizedTime, 1f);
            this.vulcanAnimator.SetTrigger(this.turbineStopID);
            this.isRunningTurbine = false;
            base.enabled = false;
        }

        private void Update()
        {
            this.currentSpeed += this.currentAcceleration * Time.deltaTime;
            this.currentSpeed = Mathf.Clamp(this.currentSpeed, 0f, (float) this.turbineMaxSpeedCoeff);
            this.vulcanAnimator.SetFloat(this.turbineCoeffID, this.currentSpeed);
            if (this.currentSpeed <= 0f)
            {
                this.StopTurbine();
            }
        }
    }
}

