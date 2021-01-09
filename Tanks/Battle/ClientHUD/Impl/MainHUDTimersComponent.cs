namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class MainHUDTimersComponent : BehaviourComponent
    {
        [SerializeField]
        private Tanks.Battle.ClientHUD.Impl.Timer timer;
        [SerializeField]
        private Tanks.Battle.ClientHUD.Impl.Timer warmingUpTimer;
        [SerializeField]
        private Animator hudAnimator;
        [SerializeField]
        private GameObject warmingUpTimerContainer;
        [SerializeField]
        private GameObject mainTimerContainer;

        public void HideWarmingUpTimer()
        {
            this.warmingUpTimerContainer.SetActive(false);
            this.mainTimerContainer.SetActive(true);
        }

        private void OnDisable()
        {
            this.HideWarmingUpTimer();
        }

        public void ShowWarmingUpTimer()
        {
            this.warmingUpTimerContainer.SetActive(true);
            this.mainTimerContainer.SetActive(false);
        }

        public Tanks.Battle.ClientHUD.Impl.Timer Timer =>
            this.timer;

        public Tanks.Battle.ClientHUD.Impl.Timer WarmingUpTimer =>
            this.warmingUpTimer;
    }
}

