namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ExitBattleWindow : LocalizedControl
    {
        [SerializeField]
        private TextMeshProUGUI yesText;
        [SerializeField]
        private TextMeshProUGUI noText;
        [SerializeField]
        private TextMeshProUGUI headerText;
        [SerializeField]
        private TextMeshProUGUI firstLineText;
        [SerializeField]
        private TextMeshProUGUI secondLineText;
        [SerializeField]
        private TextMeshProUGUI thirdLineText;
        [SerializeField]
        private TextMeshProUGUI warningText;
        [SerializeField]
        private Button yes;
        [SerializeField]
        private Button no;
        [SerializeField]
        private Color newbieHeaderColor;
        [SerializeField]
        private Color regularHeaderColor;
        [SerializeField]
        private Image warningSign;
        private Entity screen;
        private Entity battleUser;
        private bool igoreFirstEscape;
        private bool alive;
        private CursorLockMode savedLockMode;
        private bool savedCursorVisible;

        public void Hide()
        {
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetBool("Visible", false);
            }
            this.RestoreCurorState();
        }

        private void OnApplicationQuit()
        {
            this.alive = false;
        }

        private void OnDisable()
        {
            if (this.alive)
            {
                InputManager.Resume();
                if (this.screen.HasComponent<LockedScreenComponent>())
                {
                    this.screen.RemoveComponent<LockedScreenComponent>();
                }
                ECSBehaviour.EngineService.Engine.ScheduleEvent<BattleInputContextSystem.CheckMouseEvent>(this.battleUser);
            }
        }

        private void OnNo()
        {
            this.Hide();
        }

        private void OnYes()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<GoBackFromBattleEvent>(this.screen);
            this.Hide();
        }

        private void RestoreCurorState()
        {
            Cursor.lockState = this.savedLockMode;
            Cursor.visible = this.savedCursorVisible;
        }

        private void SaveCursorStateAndShow()
        {
            this.savedLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
            this.savedCursorVisible = Cursor.visible;
            Cursor.visible = true;
        }

        public void Show(Entity screen, Entity battleUser, bool customBattle, bool isDeserter, bool isNewbieBattle)
        {
            this.screen = screen;
            this.battleUser = battleUser;
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            this.SaveCursorStateAndShow();
            InputManager.Suspend();
            if (InputMapping.Cancel)
            {
                this.igoreFirstEscape = true;
            }
            base.gameObject.SetActive(true);
            this.no.GetComponent<Animator>().ResetTrigger("Normal");
            this.no.GetComponent<Animator>().SetTrigger("Highlighted");
            this.no.Select();
            if (!screen.HasComponent<LockedScreenComponent>())
            {
                screen.AddComponent<LockedScreenComponent>();
            }
            bool flag = !battleUser.HasComponent<UserInBattleAsTankComponent>();
            this.firstLineText.gameObject.SetActive(false);
            if (isNewbieBattle)
            {
                this.warningSign.gameObject.SetActive(false);
                this.headerText.color = this.newbieHeaderColor;
                this.headerText.text = this.NewbieExitText;
                this.secondLineText.text = this.NewbieSecondLineText;
                this.thirdLineText.text = this.NewbieThirdLineText;
                this.warningText.gameObject.SetActive(false);
                this.yesText.text = this.CustomYesText;
            }
            else
            {
                this.warningSign.gameObject.SetActive(true);
                this.headerText.color = this.regularHeaderColor;
                this.headerText.text = this.RegularHeaderText;
                this.secondLineText.gameObject.SetActive(!flag);
                this.secondLineText.text = !customBattle ? this.SecondLineText : this.CustomBattleSecondLineText;
                this.thirdLineText.text = (customBattle || flag) ? this.CustomThirdLineText : this.ThirdLineText;
                this.yesText.text = (customBattle || (flag || !isDeserter)) ? this.CustomYesText : this.YesText;
                this.warningText.gameObject.SetActive((isDeserter && !customBattle) && !flag);
            }
        }

        private void Start()
        {
            this.alive = true;
            this.yes.onClick.AddListener(new UnityAction(this.OnYes));
            this.no.onClick.AddListener(new UnityAction(this.OnNo));
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                if (this.igoreFirstEscape)
                {
                    this.igoreFirstEscape = false;
                    return;
                }
                this.OnNo();
            }
            this.yes.interactable ??= true;
            this.no.interactable ??= true;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public string YesText { get; set; }

        public string CustomYesText { get; set; }

        public string NoText
        {
            set => 
                this.noText.text = value;
        }

        public string FirstLineText { get; set; }

        public string SecondLineText { get; set; }

        public string CustomBattleSecondLineText { get; set; }

        public string ThirdLineText { get; set; }

        public string CustomThirdLineText { get; set; }

        public string WarningText
        {
            set => 
                this.warningText.text = value;
        }

        public string RegularHeaderText { get; set; }

        public string NewbieExitText { get; set; }

        public string NewbieSecondLineText { get; set; }

        public string NewbieThirdLineText { get; set; }
    }
}

