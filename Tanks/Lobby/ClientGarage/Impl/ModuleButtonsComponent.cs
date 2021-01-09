namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ModuleButtonsComponent : LocalizedControl, Component
    {
        [SerializeField]
        private Button mountButton;
        [SerializeField]
        private Button unmountButton;
        [SerializeField]
        private Button assembleButton;
        [SerializeField]
        private Button addResButton;
        private Entity selectedModule;
        private Entity selectedSlot;
        [SerializeField]
        private Text assembleText;
        [SerializeField]
        private Text mountText;
        [SerializeField]
        private Text unmountText;

        private void AddResources()
        {
            ShowGarageItemEvent eventInstance = new ShowGarageItemEvent {
                Item = Flow.Current.EntityRegistry.GetEntity(-370755132L)
            };
            base.ScheduleEvent(eventInstance, this.selectedModule);
        }

        protected override void Awake()
        {
            base.Awake();
            this.mountButton.onClick.AddListener(new UnityAction(this.ScheduleForModuleAndSlotEvent<ModuleMountEvent>));
            this.unmountButton.onClick.AddListener(new UnityAction(this.ScheduleForModuleAndSlotEvent<UnmountModuleFromSlotEvent>));
            this.assembleButton.onClick.AddListener(new UnityAction(this.ScheduleForModuleEvent<RequestModuleAssembleEvent>));
            this.addResButton.onClick.AddListener(new UnityAction(this.AddResources));
        }

        private void ScheduleForModuleAndSlotEvent<T>() where T: Event, new()
        {
            Entity[] entities = new Entity[] { this.selectedModule, this.selectedSlot };
            base.NewEvent<T>().AttachAll(entities).Schedule();
        }

        private void ScheduleForModuleEvent<T>() where T: Event, new()
        {
            base.ScheduleEvent<T>(this.selectedModule);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public Button MountButton =>
            this.mountButton;

        public Button UnmountButton =>
            this.unmountButton;

        public Button AssembleButton =>
            this.assembleButton;

        public Button AddResButton =>
            this.addResButton;

        public Entity SelectedModule
        {
            set => 
                this.selectedModule = value;
        }

        public Entity SelectedSlot
        {
            set => 
                this.selectedSlot = value;
        }

        public string AssembleText
        {
            get => 
                this.assembleText.text;
            set => 
                this.assembleText.text = value;
        }

        public string MountText
        {
            get => 
                this.mountText.text;
            set => 
                this.mountText.text = value;
        }

        public string UnmountText
        {
            get => 
                this.unmountText.text;
            set => 
                this.unmountText.text = value;
        }
    }
}

