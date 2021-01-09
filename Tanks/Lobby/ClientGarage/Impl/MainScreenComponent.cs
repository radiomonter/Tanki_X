namespace Tanks.Lobby.ClientGarage.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientQuests.Impl;
    using TMPro;
    using UnityEngine;

    public class MainScreenComponent : BehaviourComponent, NoScaleScreen
    {
        private int cardsCount;
        private int notificationsCount;
        private static MainScreenComponent instance;
        private readonly HashSet<IPanelShowListener> panelListeners = new HashSet<IPanelShowListener>();
        [SerializeField]
        private ItemSelectUI itemSelect;
        [SerializeField]
        private GameObject backButton;
        [SerializeField]
        private TextMeshProUGUI modeTitleInSearchingScreen;
        [SerializeField]
        private GameObject deserterIcon;
        [SerializeField]
        private DeserterDescriptionUIComponent deserterDesc;
        [SerializeField]
        private LocalizedField deserterDescLocalized;
        [SerializeField]
        private LocalizedField battlesDef;
        [SerializeField]
        private LocalizedField battlesOne;
        [SerializeField]
        private LocalizedField battlesTwo;
        [SerializeField]
        private GameObject starterPackButton;
        [SerializeField]
        private GameObject starterPackScreen;
        [SerializeField]
        private GameObject questsBtn;
        [SerializeField]
        private GameObject dailyBonusBtn;
        private Stack<HistoryItem> history = new Stack<HistoryItem>();
        private HistoryItem currentlyShown;
        private bool currentlyShownAddToHistory;
        private Action onBackOverride;
        private bool locked;
        public GameObject playButton;
        private Animator animator;
        private MainScreens lastPanel = MainScreens.Main;
        public Entity lastSelectedGameModeId;
        private ILog log = LoggerProvider.GetLogger<MainScreenComponent>();
        private IDictionary<string, Animator> animators = new Dictionary<string, Animator>();
        private bool noReset;
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated]
        private static Action <>f__am$cache1;

        public void AddListener(IPanelShowListener listener)
        {
            if (!this.panelListeners.Contains(listener))
            {
                this.panelListeners.Add(listener);
            }
        }

        private void Awake()
        {
            this.animator = base.GetComponent<Animator>();
            instance = this;
        }

        public void ClearHistory()
        {
            this.currentlyShown = null;
            this.history.Clear();
        }

        public void ClearOnBackOverride()
        {
            this.onBackOverride = null;
        }

        public void DisableReset()
        {
            this.noReset = true;
        }

        public MainScreens GetCurrentPanel() => 
            !this.animator.isActiveAndEnabled ? this.lastPanel : ((MainScreens) this.animator.GetInteger("ShowPanel"));

        public void GoBack()
        {
            if (this.onBackOverride != null)
            {
                this.onBackOverride();
            }
            else if (!this.locked)
            {
                if (!this.IsCurrentScreenInHistory())
                {
                    this.ShowLast();
                }
                else
                {
                    if (this.currentlyShown.OnGoFromThis != null)
                    {
                        this.currentlyShown.OnGoFromThis();
                    }
                    if (this.currentlyShown.OnBackFromThis != null)
                    {
                        this.currentlyShown.OnBackFromThis();
                    }
                    this.currentlyShown = this.history.Pop();
                    if (this.currentlyShown.OnBackToThis != null)
                    {
                        this.currentlyShown.OnBackToThis();
                    }
                    this.currentlyShown.Action();
                    this.UpdateBackButton();
                }
            }
        }

        public bool HasHistory() => 
            this.history.Count != 0;

        public void HideDeserterDesc()
        {
            this.deserterDesc.Hide();
        }

        public void HideDeserterIcon()
        {
            this.deserterIcon.SetActive(false);
        }

        public void HideNewItemNotification()
        {
            if ((this.cardsCount < 0) || (this.notificationsCount < 0))
            {
                this.cardsCount = 0;
                this.notificationsCount = 0;
            }
            if ((this.cardsCount == 0) && (this.notificationsCount == 0))
            {
                this.animator.SetBool("Cards", false);
            }
            this.OnPanelShow(this.GetCurrentPanel());
        }

        public void HideQuestsIfVisible()
        {
            QuestWindowComponent component = FindObjectOfType<Dialogs60Component>().Get<QuestWindowComponent>();
            if (component.gameObject.activeSelf)
            {
                component.HideWindow();
            }
        }

        public void HideShareEnergyScreen()
        {
            this.ClearOnBackOverride();
            this.ShowLast();
        }

        private bool IsCurrentScreenInHistory()
        {
            bool flag = true;
            if (Enum.IsDefined(typeof(MainScreens), this.currentlyShown.Key))
            {
                MainScreens screens = (MainScreens) Enum.Parse(typeof(MainScreens), this.currentlyShown.Key);
                if (this.GetCurrentPanel() != screens)
                {
                    flag = false;
                }
            }
            return flag;
        }

        public void Lock()
        {
            this.locked = true;
        }

        private void OnEnable()
        {
            this.SetPanel(this.lastPanel);
            if (!this.animator.enabled)
            {
                this.animator.enabled = true;
                base.GetComponent<CanvasGroup>().alpha = 1f;
            }
        }

        private void OnGUI()
        {
            if (Event.current.isMouse)
            {
                this.SendEvent<HangarCameraDelayAutoRotateEvent>(null);
            }
        }

        public void OnPanelShow(MainScreens screen)
        {
            foreach (IPanelShowListener listener in this.panelListeners)
            {
                listener.OnPanelShow(screen);
            }
        }

        public void OverrideOnBack(Action onBack)
        {
            this.onBackOverride = onBack;
        }

        private string Pluralize(int amount)
        {
            CaseType @case = CasesUtil.GetCase(amount);
            if (@case == CaseType.DEFAULT)
            {
                return this.battlesDef.Value;
            }
            if (@case == CaseType.ONE)
            {
                return this.battlesOne.Value;
            }
            if (@case != CaseType.TWO)
            {
                throw new Exception("ivnalid case");
            }
            return this.battlesTwo.Value;
        }

        public void RegisterScreen(string screenName, Animator screenAnimator)
        {
            this.animators[screenName] = screenAnimator;
        }

        public void SendShowScreenStat(LogScreen screen)
        {
            base.ScheduleEvent(new ChangeScreenLogEvent(screen), EngineService.EntityStub);
        }

        public void SetOnBackCallback(Action callback)
        {
            this.currentlyShown.OnBackToThis = callback;
        }

        private void SetPanel(MainScreens screen)
        {
            this.log.InfoFormat("SetPanel {0}", screen);
            switch (screen)
            {
                case MainScreens.MatchLobby:
                    this.SendShowScreenStat(LogScreen.BattleLobby);
                    break;

                case MainScreens.MatchSearching:
                    this.SendShowScreenStat(LogScreen.SearchBattle);
                    break;

                case MainScreens.Main:
                    this.SendShowScreenStat(LogScreen.Main);
                    break;

                case MainScreens.Parts:
                    this.SendShowScreenStat(LogScreen.Garage);
                    break;

                case MainScreens.CreateBattle:
                    this.SendShowScreenStat(LogScreen.CreateCustomBattle);
                    break;

                case MainScreens.StarterPack:
                    this.SendShowScreenStat(LogScreen.StarterPack);
                    break;

                default:
                    if (screen == MainScreens.CustomBattleScreen)
                    {
                        this.SendShowScreenStat(LogScreen.CustomBattles);
                    }
                    else if (screen == MainScreens.PlayScreen)
                    {
                        this.SendShowScreenStat(LogScreen.GameModes);
                    }
                    break;
            }
            if (this.animator.isActiveAndEnabled)
            {
                this.animator.SetInteger("ShowPanel", (int) screen);
            }
            this.lastPanel = screen;
            this.OnPanelShow(screen);
        }

        public void ShowCardsNotification(bool cards)
        {
            base.GetComponent<Animator>().SetBool("Cards", cards);
            this.OnPanelShow(MainScreens.Cards);
        }

        public void ShowCustomBattleScreen()
        {
            this.ShowScreen(MainScreens.CustomBattleScreen, true);
        }

        public void ShowCustomization()
        {
            this.ShowScreen(MainScreens.Customization, false);
        }

        public void ShowDeserterDesc(int battlesCount, bool inLobby)
        {
            this.deserterDesc.Rect.anchoredPosition = !inLobby ? new Vector2(this.deserterDesc.Rect.anchoredPosition.x, -170f) : new Vector2(this.deserterDesc.Rect.anchoredPosition.x, -187f);
            this.deserterDesc.ShowDescription(this.deserterDescLocalized.Value.Replace("{0}", battlesCount.ToString()).Replace("{1}", this.Pluralize(battlesCount)));
        }

        public void ShowDesertIcon(int battlesCount)
        {
            this.deserterIcon.SetActive(true);
            TooltipShowBehaviour component = this.deserterIcon.GetComponent<TooltipShowBehaviour>();
            component.TipText = component.localizedTip.Value.Replace("{0}", battlesCount.ToString()).Replace("{1}", this.Pluralize(battlesCount));
        }

        public void ShowHistoryItem(HistoryItem item, bool addToHistory = true)
        {
            if ((this.currentlyShown != null) && (this.currentlyShown.Key != item.Key))
            {
                if (this.currentlyShown.OnGoFromThis != null)
                {
                    this.currentlyShown.OnGoFromThis();
                }
                if (this.currentlyShownAddToHistory)
                {
                    this.history.Push(this.currentlyShown);
                }
            }
            bool flag = false;
            foreach (HistoryItem item2 in this.history)
            {
                if (item2.Key == item.Key)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                while (this.history.Pop().Key != item.Key)
                {
                }
            }
            this.log.InfoFormat("Show {0}", item.Key);
            this.currentlyShown = item;
            this.currentlyShownAddToHistory = addToHistory;
            this.currentlyShown.Action();
            this.UpdateBackButton();
        }

        public void ShowHome()
        {
            if ((this.currentlyShown != null) && (this.currentlyShown.OnGoFromThis != null))
            {
                this.currentlyShown.OnGoFromThis();
            }
            this.ClearHistory();
            Instance.ShowNewUI();
            Instance.ShowMain();
        }

        public void ShowHulls()
        {
            this.ShowHulls(this.MountedHull);
        }

        public void ShowHulls(TankPartItem selected)
        {
            <ShowHulls>c__AnonStorey2 storey = new <ShowHulls>c__AnonStorey2 {
                selected = selected,
                $this = this
            };
            HistoryItem item = new HistoryItem {
                Key = "ShowHulls",
                OnBackFromThis = new Action(storey.<>m__0),
                Action = new Action(storey.<>m__1)
            };
            this.ShowHistoryItem(item, true);
        }

        public void ShowLast()
        {
            if (this.currentlyShown == null)
            {
                this.ShowMain();
            }
            else
            {
                if (this.currentlyShown.OnBackToThis != null)
                {
                    this.currentlyShown.OnBackToThis();
                }
                this.currentlyShown.Action();
                this.UpdateBackButton();
            }
        }

        public void ShowMain()
        {
            HistoryItem item = new HistoryItem {
                Key = "Main",
                Action = delegate {
                    this.SendEvent<ResetPreviewEvent>(null);
                    this.SetPanel(MainScreens.Main);
                }
            };
            this.ShowHistoryItem(item, true);
        }

        public void ShowMatchSearching(string gameModeTitle = "")
        {
            if (!string.IsNullOrEmpty(gameModeTitle))
            {
                this.modeTitleInSearchingScreen.text = gameModeTitle;
            }
            this.ShowScreen(MainScreens.MatchSearching, false);
        }

        public void ShowNewUI()
        {
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(SingleNode<MainScreenComponent>), null);
            if (Flow.Current.NodeCollector.GetEntities(orCreateNodeClassDescription.NodeDescription).Count == 0)
            {
                base.ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(EngineService.EntityStub);
            }
        }

        public void ShowOrHideScreen(MainScreens screen, bool addToHistory = true)
        {
            if (this.GetCurrentPanel() == screen)
            {
                this.ShowMain();
            }
            else
            {
                this.ShowScreen(screen, addToHistory);
            }
        }

        public void ShowParts()
        {
            MainScreens currentPanel = this.GetCurrentPanel();
            if (((currentPanel == MainScreens.HullsAndTurrets) || (currentPanel == MainScreens.Customization)) || (currentPanel == MainScreens.Parts))
            {
                this.ShowMain();
            }
            else
            {
                HistoryItem item = new HistoryItem {
                    Key = "ShowParts",
                    Action = delegate {
                        if (this.noReset)
                        {
                            this.noReset = false;
                        }
                        else
                        {
                            this.SendEvent<ResetPreviewEvent>(null);
                        }
                        this.SetPanel(MainScreens.Parts);
                    }
                };
                this.ShowHistoryItem(item, true);
            }
        }

        public void ShowPlayScreen()
        {
            this.ShowScreen(MainScreens.PlayScreen, true);
        }

        public void ShowScreen(string screenName, bool addToHistory = true)
        {
            <ShowScreen>c__AnonStorey1 storey = new <ShowScreen>c__AnonStorey1 {
                $this = this
            };
            if (!this.animators.TryGetValue(screenName, out storey.screenAnimator))
            {
                throw new Exception("Screen not registered: " + screenName);
            }
            HistoryItem item = new HistoryItem {
                Key = screenName,
                Action = new Action(storey.<>m__0),
                OnGoFromThis = new Action(storey.<>m__1)
            };
            this.ShowHistoryItem(item, addToHistory);
        }

        public void ShowScreen(MainScreens screen, bool addToHistory = true)
        {
            <ShowScreen>c__AnonStorey0 storey = new <ShowScreen>c__AnonStorey0 {
                screen = screen,
                $this = this
            };
            if (!addToHistory)
            {
                this.SetPanel(storey.screen);
                this.UpdateBackButton();
            }
            else
            {
                HistoryItem item = new HistoryItem {
                    Key = storey.screen.ToString(),
                    Action = new Action(storey.<>m__0)
                };
                this.ShowHistoryItem(item, true);
            }
        }

        public void ShowShareEnergyScreen()
        {
            this.ShowScreen(MainScreens.ShareEnergyScreen, false);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                };
            }
            this.OverrideOnBack(<>f__am$cache0);
        }

        public void ShowShop()
        {
            if (base.GetComponent<Animator>().GetInteger("ShowPanel") == 3)
            {
                this.ShowMain();
            }
            else
            {
                this.ShowShopIfNotVisible();
            }
        }

        public void ShowShopIfNotVisible()
        {
            Animator component = base.GetComponent<Animator>();
            HistoryItem item = new HistoryItem {
                Key = "Shop",
                Action = () => this.SetPanel(MainScreens.Shop)
            };
            this.ShowHistoryItem(item, true);
        }

        public void ShowStarterPack()
        {
            this.ShowScreen(MainScreens.StarterPack, false);
            this.backButton.SetActive(false);
        }

        public void ShowTurrets()
        {
            this.ShowTurrets(this.MountedTurret);
        }

        public void ShowTurrets(TankPartItem selected)
        {
            <ShowTurrets>c__AnonStorey3 storey = new <ShowTurrets>c__AnonStorey3 {
                selected = selected,
                $this = this
            };
            HistoryItem item = new HistoryItem {
                Key = "ShowTurrets",
                OnBackFromThis = new Action(storey.<>m__0),
                Action = new Action(storey.<>m__1)
            };
            this.ShowHistoryItem(item, true);
        }

        public void ToggleNews(bool showNews)
        {
            this.animator.SetBool("newsIsShow", showNews);
        }

        public void ToProfile()
        {
            if (this.GetCurrentPanel() == MainScreens.UserProfile)
            {
                this.GoBack();
            }
            else
            {
                HistoryItem item2 = new HistoryItem {
                    Key = "ShowGameModeSelect"
                };
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate {
                    };
                }
                item2.OnBackToThis = <>f__am$cache1;
                item2.Action = delegate {
                    this.animator.SetInteger("ShowPanel", -5);
                    this.SetPanel(MainScreens.UserProfile);
                };
                HistoryItem item = item2;
                this.ShowHistoryItem(item, true);
                this.backButton.SetActive(true);
            }
        }

        public void Unlock()
        {
            this.locked = false;
        }

        private void UpdateBackButton()
        {
            MainScreens currentPanel = this.GetCurrentPanel();
            if ((currentPanel != MainScreens.Main) && (currentPanel != MainScreens.Shop))
            {
                this.backButton.SetActive(this.HasHistory());
            }
            else
            {
                this.backButton.SetActive(false);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public int CardsCount
        {
            get => 
                this.cardsCount;
            set => 
                this.cardsCount = value;
        }

        public int NotificationsCount
        {
            get => 
                this.notificationsCount;
            set => 
                this.notificationsCount = value;
        }

        public static MainScreenComponent Instance =>
            instance;

        public GameObject StarterPackButton =>
            this.starterPackButton;

        public GameObject StarterPackScreen =>
            this.starterPackScreen;

        public GameObject QuestsBtn =>
            this.questsBtn;

        public GameObject DailyBonusBtn =>
            this.dailyBonusBtn;

        public TankPartItem MountedHull { get; set; }

        public TankPartItem MountedTurret { get; set; }

        public Action OnBack =>
            this.onBackOverride;

        [CompilerGenerated]
        private sealed class <ShowHulls>c__AnonStorey2
        {
            internal TankPartItem selected;
            internal MainScreenComponent $this;

            internal void <>m__0()
            {
                if (!this.$this.itemSelect.IsSelected)
                {
                    this.selected.Select();
                }
            }

            internal void <>m__1()
            {
                this.$this.SendShowScreenStat(LogScreen.Hulls);
                this.$this.SetPanel(MainScreenComponent.MainScreens.HullsAndTurrets);
                this.$this.itemSelect.SetItems(MainScreenComponent.GarageItemsRegistry.Hulls, this.selected);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowScreen>c__AnonStorey0
        {
            internal MainScreenComponent.MainScreens screen;
            internal MainScreenComponent $this;

            internal void <>m__0()
            {
                this.$this.SetPanel(this.screen);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowScreen>c__AnonStorey1
        {
            internal Animator screenAnimator;
            internal MainScreenComponent $this;

            internal void <>m__0()
            {
                this.$this.SetPanel(MainScreenComponent.MainScreens.Hide);
                this.screenAnimator.gameObject.SetActive(true);
                this.screenAnimator.SetBool("show", true);
            }

            internal void <>m__1()
            {
                this.screenAnimator.SetBool("show", false);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowTurrets>c__AnonStorey3
        {
            internal TankPartItem selected;
            internal MainScreenComponent $this;

            internal void <>m__0()
            {
                if (!this.$this.itemSelect.IsSelected)
                {
                    this.selected.Select();
                }
            }

            internal void <>m__1()
            {
                this.$this.SendShowScreenStat(LogScreen.Turrets);
                this.$this.SetPanel(MainScreenComponent.MainScreens.HullsAndTurrets);
                this.$this.itemSelect.SetItems(MainScreenComponent.GarageItemsRegistry.Turrets, this.selected);
            }
        }

        public class HistoryItem
        {
            public string Key;
            public System.Action Action;
            public System.Action OnBackToThis;
            public System.Action OnGoFromThis;
            public System.Action OnBackFromThis;
        }

        public interface IPanelShowListener
        {
            void OnPanelShow(MainScreenComponent.MainScreens screen);
        }

        public enum MainScreens
        {
            CustomBattleScreen = -21,
            PlayScreen = -20,
            UserProfile = -5,
            MatchLobby = -4,
            MatchSearching = -3,
            GameMode = -2,
            Main = -1,
            Parts = 0,
            HullsAndTurrets = 1,
            Customization = 2,
            Shop = 3,
            CreateBattle = 4,
            Cards = 5,
            StarterPack = 6,
            ShareEnergyScreen = 7,
            TankRent = 8,
            Hide = 0x63
        }
    }
}

