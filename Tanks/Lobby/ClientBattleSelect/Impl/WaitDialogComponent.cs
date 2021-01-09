namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class WaitDialogComponent : UIBehaviour, Component
    {
        public float maxTimerValue = 5f;
        private float _timer;
        [SerializeField]
        private Slider timerSlider;
        [SerializeField]
        private TextMeshProUGUI message;
        private CursorLockMode savedLockMode;
        private bool savedCursorVisible;
        private bool isShow;

        public void Hide()
        {
            this.IsShow = false;
            MainScreenComponent.Instance.Unlock();
            Destroy(base.gameObject, 3f);
        }

        private void OnHideAnimationEvent()
        {
            if (!this.IsShow)
            {
                base.gameObject.SetActive(false);
            }
        }

        public virtual void Show(string messageText)
        {
            this.timer = 0f;
            MainScreenComponent.Instance.Lock();
            this.message.text = messageText;
            base.gameObject.SetActive(true);
            this.IsShow = true;
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            if (this.timer > this.maxTimerValue)
            {
                this.Hide();
            }
        }

        private float timer
        {
            get => 
                this._timer;
            set
            {
                this._timer = value;
                this.timerSlider.value = 1f - (this.timer / this.maxTimerValue);
            }
        }

        private bool IsShow
        {
            get => 
                this.isShow;
            set
            {
                base.GetComponent<Animator>().SetBool("show", value);
                this.isShow = value;
            }
        }
    }
}

