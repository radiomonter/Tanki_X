namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class QuestProgressGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private AnimatedProgress progress;
        [SerializeField]
        private TextMeshProUGUI currentProgressValue;
        [SerializeField]
        private TextMeshProUGUI targetProgressValue;
        [SerializeField]
        private TextMeshProUGUI deltaProgressValue;
        [SerializeField]
        private Animator deltaProgressAnimator;

        private void DisableOutsideClickingOption()
        {
        }

        private void EnableOutsideClickingOption()
        {
        }

        public void Initialize(float currentValue, float targetValue)
        {
            this.progress.InitialNormalizedValue = currentValue / targetValue;
            this.CurrentProgressValue = currentValue.ToString();
        }

        public void Set(float currentValue, float targetValue)
        {
            this.progress.NormalizedValue = currentValue / targetValue;
            this.CurrentProgressValue = currentValue.ToString();
        }

        public string TargetProgressValue
        {
            get => 
                this.targetProgressValue.text;
            set => 
                this.targetProgressValue.text = value;
        }

        public string CurrentProgressValue
        {
            get => 
                this.currentProgressValue.text;
            set => 
                this.currentProgressValue.text = value;
        }

        public string DeltaProgressValue
        {
            get => 
                this.deltaProgressValue.text;
            set
            {
                this.deltaProgressValue.text = value;
                this.deltaProgressAnimator.SetTrigger("ShowProgressDelta");
            }
        }
    }
}

