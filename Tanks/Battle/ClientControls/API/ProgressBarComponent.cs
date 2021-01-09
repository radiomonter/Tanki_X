namespace Tanks.Battle.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ProgressBarComponent : BehaviourComponent
    {
        [SerializeField]
        private float progressValueOffset;
        private Tanks.Lobby.ClientControls.API.ProgressBar progressBar;
        private float targetProgresValue;

        private float ClampProgressValue(float realValue, float offset)
        {
            realValue = Mathf.Clamp01(realValue);
            return (((realValue == 0f) || (realValue == 1f)) ? realValue : ((realValue >= offset) ? ((realValue <= (1f - offset)) ? realValue : (1f - offset)) : offset));
        }

        protected virtual void Update()
        {
            if ((this.ProgressBar.ProgressValue <= this.targetProgresValue) && (this.targetProgresValue <= 0.9f))
            {
                this.ProgressBar.ProgressValue = Mathf.Clamp(this.ProgressBar.ProgressValue + (Time.deltaTime * 0.05f), this.ProgressBar.ProgressValue, this.targetProgresValue);
            }
            else
            {
                this.ProgressBar.ProgressValue = this.targetProgresValue;
            }
        }

        public virtual float ProgressValue
        {
            get => 
                this.targetProgresValue;
            set
            {
                this.targetProgresValue = this.ClampProgressValue(value, this.progressValueOffset);
                this.ProgressBar.ProgressValue = this.targetProgresValue;
            }
        }

        public Tanks.Lobby.ClientControls.API.ProgressBar ProgressBar
        {
            get
            {
                if (this.progressBar == null)
                {
                    this.progressBar = base.GetComponent<Tanks.Lobby.ClientControls.API.ProgressBar>();
                }
                return this.progressBar;
            }
        }
    }
}

