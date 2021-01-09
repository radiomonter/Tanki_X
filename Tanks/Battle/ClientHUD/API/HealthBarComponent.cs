namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientControls.API;
    using UnityEngine;

    public class HealthBarComponent : ProgressBarComponent
    {
        private float actualValue;
        private float animatedValue;
        private bool animating;
        private float animationTime;
        private CanvasGroup _canvasGroup;
        private float _defaultAlpha;

        public void ActivateAnimation(float timeInSec)
        {
            this.animationTime = timeInSec;
            this.animating = true;
            this.actualValue = base.ProgressValue;
            this.animatedValue = 0f;
        }

        public void HideHealthBar()
        {
            this._canvasGroup.alpha = 0f;
            base.gameObject.transform.SetAsFirstSibling();
        }

        private void OnEnable()
        {
            this._canvasGroup = base.GetComponent<CanvasGroup>();
            this._defaultAlpha = this._canvasGroup.alpha;
        }

        public void ShowHealthBar()
        {
            this._canvasGroup.alpha = this._defaultAlpha;
            base.gameObject.transform.SetAsLastSibling();
        }

        protected override void Update()
        {
            base.Update();
            if (this.animating)
            {
                this.animatedValue += Time.deltaTime / this.animationTime;
                if (this.animatedValue >= 1f)
                {
                    this.animating = false;
                    base.ProgressValue = this.actualValue;
                }
                base.ProgressValue = this.animatedValue;
            }
        }

        [Inject]
        public static UnityTime Time { get; set; }

        public override float ProgressValue
        {
            get => 
                !this.animating ? base.ProgressValue : this.actualValue;
            set
            {
                if (this.animating)
                {
                    this.actualValue = value;
                }
                else
                {
                    base.ProgressValue = value;
                }
            }
        }
    }
}

