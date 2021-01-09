namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class CustomizationUIComponent : BehaviourComponent
    {
        private TankPartItem selectedItem;
        [SerializeField]
        private VisualUI visualUI;
        [SerializeField]
        private ModulesScreenUIComponent modulesScreenUI;
        [SerializeField]
        private GarageSelectorUI garageSelectorUI;
        private string delayedTrigger;
        private int visualTab;

        public void HullModules()
        {
            this.selectedItem = this.Hull;
            GoToModulesScreenEvent eventInstance = new GoToModulesScreenEvent(TankPartModuleType.TANK);
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            this.MainScreen.SendShowScreenStat(LogScreen.TurretModules);
        }

        public void HullVisual(int tab)
        {
            <HullVisual>c__AnonStorey0 storey = new <HullVisual>c__AnonStorey0 {
                tab = tab,
                $this = this
            };
            MainScreenComponent.HistoryItem item = new MainScreenComponent.HistoryItem {
                OnGoFromThis = new Action(this.visualUI.ReturnCameraOffset),
                Key = "Customization",
                Action = new Action(storey.<>m__0)
            };
            this.MainScreen.ShowHistoryItem(item, true);
        }

        public void HullVisualNoSwitch(int tab)
        {
            <HullVisualNoSwitch>c__AnonStorey3 storey = new <HullVisualNoSwitch>c__AnonStorey3 {
                tab = tab,
                $this = this
            };
            MainScreenComponent.HistoryItem item = new MainScreenComponent.HistoryItem {
                OnGoFromThis = new Action(this.visualUI.ReturnCameraOffset),
                Key = "CustomizationNoSwitch",
                Action = new Action(storey.<>m__0)
            };
            this.MainScreen.ShowHistoryItem(item, true);
        }

        public void Modules()
        {
            if (ReferenceEquals(this.selectedItem, this.Turret))
            {
                this.TurretModules();
            }
            else
            {
                this.HullModules();
            }
        }

        private void OnDisable()
        {
            this.visualUI.gameObject.SetActive(false);
            this.modulesScreenUI.gameObject.SetActive(false);
            this.visualUI.ReturnCameraOffset();
        }

        private void OnEnable()
        {
            if (this.delayedTrigger != null)
            {
                base.GetComponent<Animator>().SetTrigger(this.delayedTrigger);
                this.delayedTrigger = null;
            }
        }

        private void OnHullTechSelected()
        {
            this.HullModules();
        }

        private void OnHullVisualSelected()
        {
            this.HullVisual(this.visualTab);
        }

        private void OnTurretTechSelected()
        {
            this.TurretModules();
        }

        private void OnTurretVisualSelected()
        {
            this.TurretVisual(this.visualTab);
        }

        public void SetVisualTab(int tab)
        {
            <SetVisualTab>c__AnonStorey6 storey = new <SetVisualTab>c__AnonStorey6 {
                tab = tab,
                $this = this
            };
            if (storey.tab >= 0)
            {
                this.visualTab = storey.tab;
                MainScreenComponent.Instance.SetOnBackCallback(new Action(storey.<>m__0));
            }
        }

        private void ShowTech(TankPartItem item)
        {
            <ShowTech>c__AnonStorey5 storey = new <ShowTech>c__AnonStorey5 {
                item = item,
                $this = this
            };
            this.selectedItem = storey.item;
            if (this.modulesScreenUI.gameObject.activeInHierarchy)
            {
                this.modulesScreenUI.SetItem(storey.item);
            }
            else
            {
                this.modulesScreenUI.onEanble = new Action(storey.<>m__0);
                if (base.gameObject.activeInHierarchy)
                {
                    base.GetComponent<Animator>().SetTrigger("ShowTech");
                }
                else
                {
                    this.delayedTrigger = "ShowTech";
                }
            }
        }

        private void ShowVisual(TankPartItem item, int visualTab = 0)
        {
            <ShowVisual>c__AnonStorey4 storey = new <ShowVisual>c__AnonStorey4 {
                item = item,
                visualTab = visualTab,
                $this = this
            };
            this.selectedItem = storey.item;
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetTrigger("ShowVisual");
            }
            else
            {
                this.delayedTrigger = "ShowVisual";
            }
            this.visualUI.onEanble = new Action(storey.<>m__0);
        }

        public void TurretModules()
        {
            this.selectedItem = this.Turret;
            GoToModulesScreenEvent eventInstance = new GoToModulesScreenEvent(TankPartModuleType.WEAPON);
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            this.MainScreen.SendShowScreenStat(LogScreen.TurretModules);
        }

        public void TurretVisual(int tab)
        {
            <TurretVisual>c__AnonStorey1 storey = new <TurretVisual>c__AnonStorey1 {
                tab = tab,
                $this = this
            };
            MainScreenComponent.HistoryItem item = new MainScreenComponent.HistoryItem {
                OnGoFromThis = new Action(this.visualUI.ReturnCameraOffset),
                Key = "Customization",
                Action = new Action(storey.<>m__0)
            };
            this.MainScreen.ShowHistoryItem(item, true);
        }

        public void TurretVisualNoSwitch(int tab)
        {
            <TurretVisualNoSwitch>c__AnonStorey2 storey = new <TurretVisualNoSwitch>c__AnonStorey2 {
                tab = tab,
                $this = this
            };
            MainScreenComponent.HistoryItem item = new MainScreenComponent.HistoryItem {
                OnGoFromThis = new Action(this.visualUI.ReturnCameraOffset),
                Key = "CustomizationNoSwitch",
                Action = new Action(storey.<>m__0)
            };
            this.MainScreen.ShowHistoryItem(item, true);
        }

        public void Visual()
        {
            if (ReferenceEquals(this.selectedItem, this.Turret))
            {
                this.TurretVisual(this.visualTab);
            }
            else
            {
                this.HullVisual(this.visualTab);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public TankPartItem Hull { get; set; }

        public TankPartItem Turret { get; set; }

        private MainScreenComponent MainScreen =>
            base.GetComponentInParent<MainScreenComponent>();

        [CompilerGenerated]
        private sealed class <HullVisual>c__AnonStorey0
        {
            internal int tab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.GetComponentInParent<MainScreenComponent>().ShowCustomization();
                this.$this.garageSelectorUI.gameObject.SetActive(true);
                this.$this.SetVisualTab(this.tab);
                this.$this.garageSelectorUI.onHullSelected = new Action(this.$this.OnHullVisualSelected);
                this.$this.garageSelectorUI.onTurretSelected = new Action(this.$this.OnTurretVisualSelected);
                this.$this.garageSelectorUI.SelectVisual();
                this.$this.garageSelectorUI.SelectHull();
                this.$this.ShowVisual(this.$this.Hull, this.$this.visualTab);
            }
        }

        [CompilerGenerated]
        private sealed class <HullVisualNoSwitch>c__AnonStorey3
        {
            internal int tab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.GetComponentInParent<MainScreenComponent>().ShowCustomization();
                this.$this.garageSelectorUI.gameObject.SetActive(true);
                this.$this.SetVisualTab(this.tab);
                this.$this.garageSelectorUI.onHullSelected = new Action(this.$this.OnHullVisualSelected);
                this.$this.garageSelectorUI.onTurretSelected = new Action(this.$this.OnTurretVisualSelected);
                this.$this.garageSelectorUI.SelectVisual();
                this.$this.garageSelectorUI.SelectHull();
                this.$this.ShowVisual(this.$this.Hull, this.$this.visualTab);
                this.$this.garageSelectorUI.gameObject.SetActive(false);
            }
        }

        [CompilerGenerated]
        private sealed class <SetVisualTab>c__AnonStorey6
        {
            internal int tab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.SetVisualTab(this.tab);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowTech>c__AnonStorey5
        {
            internal TankPartItem item;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.modulesScreenUI.SetItem(this.item);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowVisual>c__AnonStorey4
        {
            internal TankPartItem item;
            internal int visualTab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.visualUI.Set(this.item, this.visualTab);
            }
        }

        [CompilerGenerated]
        private sealed class <TurretVisual>c__AnonStorey1
        {
            internal int tab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.GetComponentInParent<MainScreenComponent>().ShowCustomization();
                this.$this.garageSelectorUI.gameObject.SetActive(true);
                this.$this.SetVisualTab(this.tab);
                this.$this.garageSelectorUI.onHullSelected = new Action(this.$this.OnHullVisualSelected);
                this.$this.garageSelectorUI.onTurretSelected = new Action(this.$this.OnTurretVisualSelected);
                this.$this.garageSelectorUI.SelectVisual();
                this.$this.garageSelectorUI.SelectTurret();
                this.$this.ShowVisual(this.$this.Turret, this.$this.visualTab);
            }
        }

        [CompilerGenerated]
        private sealed class <TurretVisualNoSwitch>c__AnonStorey2
        {
            internal int tab;
            internal CustomizationUIComponent $this;

            internal void <>m__0()
            {
                this.$this.GetComponentInParent<MainScreenComponent>().ShowCustomization();
                this.$this.garageSelectorUI.gameObject.SetActive(true);
                this.$this.SetVisualTab(this.tab);
                this.$this.garageSelectorUI.onHullSelected = new Action(this.$this.OnHullVisualSelected);
                this.$this.garageSelectorUI.onTurretSelected = new Action(this.$this.OnTurretVisualSelected);
                this.$this.garageSelectorUI.SelectVisual();
                this.$this.garageSelectorUI.SelectTurret();
                this.$this.garageSelectorUI.gameObject.SetActive(false);
                this.$this.ShowVisual(this.$this.Turret, this.$this.visualTab);
            }
        }
    }
}

