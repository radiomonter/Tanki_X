namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class CircleProgressBar : MonoBehaviour
    {
        [SerializeField]
        private float animationSpeed = 1f;
        [SerializeField]
        private Image mainProgressImage;
        [SerializeField]
        private Image additionalProgressImage;
        [SerializeField]
        private Image additionalProgressContainer;
        [SerializeField]
        private Image additionalProgressImage1;
        [SerializeField]
        private Image additionalProgressContainer1;
        [SerializeField]
        private float progress;
        public Action allAnimationComplete;
        [SerializeField]
        private float additionalProgress;
        [SerializeField]
        private float additionalProgress1;
        private List<ProgressBarUpgradeAnimation> upgradeAnimations = new List<ProgressBarUpgradeAnimation>();
        private bool animated = true;

        public void AddUpgradeAnimation(float progress, float additionalProgress, float additionalProgress1 = 0f)
        {
            this.upgradeAnimations.Add(new ProgressBarUpgradeAnimation(progress, additionalProgress, additionalProgress1));
        }

        private void Animate()
        {
            this.animated = true;
        }

        public void Animate(float delay)
        {
            base.CancelInvoke();
            base.Invoke("Animate", delay);
        }

        public void ClearUpgradeAnimations()
        {
            this.upgradeAnimations = new List<ProgressBarUpgradeAnimation>();
        }

        public void ResetProgressView()
        {
            this.mainProgressImage.fillAmount = 0f;
            this.additionalProgressContainer.fillAmount = 0f;
            this.additionalProgressContainer.rectTransform.eulerAngles = Vector3.zero;
            this.additionalProgressImage.rectTransform.localEulerAngles = Vector3.zero;
            this.additionalProgressContainer1.fillAmount = 0f;
            this.additionalProgressContainer1.rectTransform.eulerAngles = Vector3.zero;
            this.additionalProgressImage1.rectTransform.localEulerAngles = Vector3.zero;
        }

        public void SelectNextUpgradeAnimation()
        {
            if (this.upgradeAnimations.Count <= 0)
            {
                if (this.allAnimationComplete != null)
                {
                    this.allAnimationComplete();
                    this.allAnimationComplete = null;
                }
            }
            else
            {
                ProgressBarUpgradeAnimation item = this.upgradeAnimations[0];
                this.upgradeAnimations.Remove(item);
                this.ResetProgressView();
                this.Progress = item.progress;
                this.AdditionalProgress = item.additionalProgress;
                this.AdditionalProgress1 = item.additionalProgress1;
            }
        }

        public void StopAnimation()
        {
            this.animated = false;
        }

        private void Update()
        {
            if (this.animated)
            {
                float num = Mathf.Abs((float) (this.progress - this.mainProgressImage.fillAmount));
                if (num != 0f)
                {
                    this.mainProgressImage.fillAmount = Mathf.Lerp(this.mainProgressImage.fillAmount, this.progress, (Time.deltaTime * this.animationSpeed) / num);
                }
                float num2 = Mathf.Abs((float) (this.additionalProgress - this.additionalProgressContainer.fillAmount));
                if (num2 != 0f)
                {
                    this.additionalProgressContainer.fillAmount = Mathf.Lerp(this.additionalProgressContainer.fillAmount, this.additionalProgress, (Time.deltaTime * this.animationSpeed) / num2);
                }
                float z = -360f * this.mainProgressImage.fillAmount;
                this.additionalProgressContainer.rectTransform.eulerAngles = new Vector3(0f, 0f, z);
                this.additionalProgressImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, -z);
                float num4 = 0f;
                if ((this.additionalProgressContainer1 != null) && (this.additionalProgressImage1 != null))
                {
                    num4 = Mathf.Abs((float) (this.additionalProgress1 - this.additionalProgressContainer1.fillAmount));
                    if (num4 != 0f)
                    {
                        this.additionalProgressContainer1.fillAmount = Mathf.Lerp(this.additionalProgressContainer1.fillAmount, this.additionalProgress1, (Time.deltaTime * this.animationSpeed) / num4);
                    }
                    z = (-360f * this.mainProgressImage.fillAmount) - (360f * this.additionalProgressContainer.fillAmount);
                    this.additionalProgressContainer1.rectTransform.eulerAngles = new Vector3(0f, 0f, z);
                    this.additionalProgressImage1.rectTransform.localEulerAngles = new Vector3(0f, 0f, -z);
                }
                if ((num == 0f) && ((num2 == num) && (num4 == num)))
                {
                    this.SelectNextUpgradeAnimation();
                }
            }
        }

        public float Progress
        {
            set
            {
                this.progress = value;
                this.additionalProgress = 0f;
                this.additionalProgress1 = 0f;
            }
        }

        public float AdditionalProgress
        {
            set => 
                this.additionalProgress = value;
        }

        public float AdditionalProgress1
        {
            set => 
                this.additionalProgress1 = value;
        }
    }
}

