namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class FlamethrowerMotionAnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private string motionCoeffName = "motionCoeff";
        [SerializeField]
        private string stopMotionName = "stopMotion";
        [SerializeField]
        private float idleMotionCoeff = -0.25f;
        [SerializeField]
        private float workingMotionCoeff = 1f;
        [SerializeField]
        private string motionStateName = "Motion";
        [SerializeField]
        private int motionStateLayerIndex;
        private int motionCoeffID;
        private int stopMotionID;
        private int motionStateID;
        private float currentNrmTime;
        private Animator flamethrowerAnimator;
        private bool isWorkingMotion;

        private void Awake()
        {
            base.enabled = false;
        }

        public void Init(Animator animator)
        {
            this.flamethrowerAnimator = animator;
            this.currentNrmTime = 0f;
            this.motionCoeffID = Animator.StringToHash(this.motionCoeffName);
            this.stopMotionID = Animator.StringToHash(this.stopMotionName);
            this.motionStateID = Animator.StringToHash(this.motionStateName);
        }

        public void StartIdleMotion()
        {
            this.StartMotion(false);
        }

        private void StartMotion(bool isWorking)
        {
            this.isWorkingMotion = isWorking;
            this.flamethrowerAnimator.ResetTrigger(this.stopMotionID);
            this.flamethrowerAnimator.SetFloat(this.motionCoeffID, !this.isWorkingMotion ? this.idleMotionCoeff : this.workingMotionCoeff);
            this.flamethrowerAnimator.Play(this.motionStateID, this.motionStateLayerIndex, this.currentNrmTime);
            base.enabled = true;
        }

        public void StartWorkingMotion()
        {
            this.StartMotion(true);
        }

        public void StopMotion()
        {
            this.flamethrowerAnimator.SetTrigger(this.stopMotionID);
            base.enabled = false;
        }

        private void Update()
        {
            float num3 = this.flamethrowerAnimator.GetCurrentAnimatorStateInfo(this.motionStateLayerIndex).normalizedTime - this.currentNrmTime;
            this.currentNrmTime += num3;
            this.currentNrmTime = Mathf.Repeat(this.currentNrmTime, 1f);
        }
    }
}

