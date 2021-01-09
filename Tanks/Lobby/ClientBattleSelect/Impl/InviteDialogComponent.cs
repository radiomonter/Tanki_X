namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.Impl;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class InviteDialogComponent : UIBehaviour, Component
    {
        [SerializeField]
        private GameObject buttons;
        [SerializeField]
        private GameObject keys;
        public float maxTimerValue = 5f;
        private float _timer;
        [SerializeField]
        private Slider timerSlider;
        private bool inBattle;
        [SerializeField]
        private TextMeshProUGUI message;
        [SerializeField]
        private Button acceptButton;
        [SerializeField]
        private Button declineButton;
        [SerializeField]
        private AudioSource sound;
        private CursorLockMode savedLockMode;
        private bool savedCursorVisible;
        private bool isShow;
        private bool intractable;

        private bool ChatIsFocused()
        {
            BattleChatFocusCheckEvent eventInstance = new BattleChatFocusCheckEvent();
            EngineService.Engine.ScheduleEvent(eventInstance, new EntityStub());
            return eventInstance.InputIsFocused;
        }

        public void Hide()
        {
            this.intractable = false;
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

        public void OnNo()
        {
            if (this.intractable)
            {
                this.Hide();
                if (base.GetComponent<EntityBehaviour>() != null)
                {
                    Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                    EngineService.Engine.ScheduleEvent<DialogDeclineEvent>(entity);
                }
            }
        }

        private void OnYes()
        {
            if (this.intractable)
            {
                this.Hide();
                if (base.GetComponent<EntityBehaviour>() != null)
                {
                    Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                    EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
                }
            }
        }

        public virtual void Show(string messageText, bool inBattle)
        {
            this.intractable = true;
            this.timer = 0f;
            MainScreenComponent.Instance.Lock();
            this.message.text = messageText;
            base.gameObject.SetActive(true);
            this.IsShow = true;
            this.inBattle = inBattle;
            this.buttons.SetActive(!inBattle);
            this.keys.SetActive(inBattle);
            if (this.sound != null)
            {
                this.sound.Play();
            }
        }

        protected override void Start()
        {
            this.acceptButton.onClick.AddListener(new UnityAction(this.OnYes));
            this.declineButton.onClick.AddListener(new UnityAction(this.OnNo));
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            if (this.timer > this.maxTimerValue)
            {
                this.OnNo();
            }
            if (InputMapping.Cancel)
            {
                this.OnNo();
            }
            else if (Input.GetKeyDown(KeyCode.Y) && (this.inBattle && !this.ChatIsFocused()))
            {
                this.OnYes();
            }
            else if (Input.GetKeyDown(KeyCode.N) && (this.inBattle && !this.ChatIsFocused()))
            {
                this.OnNo();
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

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

