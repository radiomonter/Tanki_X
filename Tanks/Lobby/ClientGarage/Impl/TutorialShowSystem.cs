namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using tanks.modules.lobby.ClientGarage.Impl;

    public class TutorialShowSystem : ECSSystem
    {
        public TutorialNode activeTutorial;

        [OnEventFire]
        public void AddItemHandler(NodeAddedEvent e, SingleNode<AddItemStepHandler> stepHandler, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            ApplyTutorialIdEvent eventInstance = new ApplyTutorialIdEvent(stepHandler.component.stepId);
            base.ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void AddItemHandler(NodeAddedEvent e, SingleNode<ResetFreeEnergyStepHandler> stepHandler, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            ApplyTutorialIdEvent eventInstance = new ApplyTutorialIdEvent(stepHandler.component.stepId);
            base.ScheduleEvent(eventInstance, session);
        }

        [OnEventFire]
        public void AddItemResult(TutorialIdResultEvent e, Node any, [JoinAll] SingleNode<ResetFreeEnergyStepHandler> stepHandler, [JoinAll] ICollection<TutorialStepNode> steps, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (stepHandler.component.stepId == e.Id)
            {
                TutorialStepNode stepById = this.GetStepById(stepHandler.component.stepId, steps);
                this.AddItemResultHandler(e, stepHandler.component, stepById, session);
            }
        }

        [OnEventFire]
        public void AddItemResult(TutorialIdResultEvent e, Node any, [JoinAll] ICollection<SingleNode<AddItemStepHandler>> stepHandlers, [JoinAll] ICollection<TutorialStepNode> steps, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            foreach (SingleNode<AddItemStepHandler> node in stepHandlers)
            {
                if (node.component.stepId == e.Id)
                {
                    TutorialStepNode stepById = this.GetStepById(node.component.stepId, steps);
                    this.AddItemResultHandler(e, node.component, stepById, session);
                }
            }
        }

        private void AddItemResultHandler(TutorialIdResultEvent e, AddItemStepHandler stepHandler, TutorialStepNode step, SingleNode<ClientSessionComponent> session)
        {
            if (!e.ActionExecuted)
            {
                stepHandler.Fail(e.Id);
            }
            else
            {
                if (step != null)
                {
                    IList<TutorialNode> source = base.Select<TutorialNode>(step.Entity, typeof(TutorialGroupComponent));
                    if (source.Count > 0)
                    {
                        TutorialNode node = source.Single<TutorialNode>();
                        base.ScheduleEvent(new ApplyTutorialIdEvent(node.tutorialData.TutorialId), session);
                    }
                }
                stepHandler.Success(e.Id);
            }
        }

        [OnEventFire]
        public void CheckBoughtWepon(CheckBoughtItemEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<UserWeaponNode> userWeapons)
        {
            <CheckBoughtWepon>c__AnonStorey1 storey = new <CheckBoughtWepon>c__AnonStorey1 {
                e = e
            };
            storey.e.TutorialItemAlreadyBought = userWeapons.Any<UserWeaponNode>(new Func<UserWeaponNode, bool>(storey.<>m__0));
        }

        [OnEventFire]
        public void CheckForActiveNotifications(CheckForActiveNotificationsEvent e, Node any, [JoinAll] ICollection<SingleNode<ReleaseGiftsNotificationComponent>> releaseGiftsNotification, [JoinAll] ICollection<SingleNode<EulaNotificationComponent>> eulaNotification, [JoinAll] ICollection<SingleNode<PrivacyPolicyNotificationComponent>> privacyPolicyNotification, [JoinAll] ICollection<SingleNode<LoginRewardsNotificationComponent>> loginRewardNotification)
        {
            e.NotificationExist = ((releaseGiftsNotification.Count > 0) || ((eulaNotification.Count > 0) || (loginRewardNotification.Count > 0))) || (privacyPolicyNotification.Count > 0);
        }

        [OnEventFire]
        public void CheckMountedModule(CheckMountedModuleEvent e, Node any, [JoinAll] ICollection<SlotNode> slots)
        {
            using (IEnumerator<SlotNode> enumerator = slots.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        SlotNode current = enumerator.Current;
                        if (current.slotUserItemInfo.Slot != e.MountSlot)
                        {
                            continue;
                        }
                        IList<UserModuleNode> list = base.Select<UserModuleNode>(current.Entity, typeof(ModuleGroupComponent));
                        if ((list.Count > 0) && (list[0].marketItemGroup.Key == e.ItemId))
                        {
                            e.ModuleMounted = true;
                            return;
                        }
                    }
                    break;
                }
            }
            e.ModuleMounted = false;
        }

        [OnEventFire]
        public void CheckTutorial(CheckForTutorialEvent e, Node any)
        {
            e.TutorialIsActive = !ReferenceEquals(this.activeTutorial, null);
        }

        [OnEventFire]
        public void CloseEmailNotification(SetNotificationVisibleEvent e, EmailConfirmationNotificationNode emailConfirmationNotification)
        {
            if (this.activeTutorial != null)
            {
                base.ScheduleEvent<CloseNotificationEvent>(emailConfirmationNotification);
            }
        }

        [OnEventFire]
        public void CompleteTutorialOnEsc(CompleteTutorialByEscEvent e, TutorialStepNode step, [JoinByTutorial] TutorialNode tutorial)
        {
            if (!tutorial.Entity.HasComponent<TutorialCompleteComponent>())
            {
                tutorial.Entity.AddComponent<TutorialCompleteComponent>();
            }
        }

        [OnEventFire]
        public void ContinueTutorialOnLoginRewardRemoved(NodeRemoveEvent e, SingleNode<LoginRewardDialog> loginRewardDialog)
        {
            TutorialCanvas.Instance.Continue();
        }

        [OnEventFire]
        public void GetChangeTurretTutorialValidationData(GetChangeTurretTutorialValidationDataEvent e, Node any, [JoinAll] MountedWeaponNode mountedWeapon, [JoinByUser] SingleNode<SelfUserComponent> selfUser, [JoinByUser] ICollection<UserWeaponNode> userWeapons)
        {
            <GetChangeTurretTutorialValidationData>c__AnonStorey0 storey = new <GetChangeTurretTutorialValidationData>c__AnonStorey0 {
                e = e
            };
            storey.e.MountedWeaponId = mountedWeapon.marketItemGroup.Key;
            storey.e.TutorialItemAlreadyMounted = mountedWeapon.marketItemGroup.Key == storey.e.ItemId;
            storey.e.TutorialItemAlreadyBought = userWeapons.Any<UserWeaponNode>(new Func<UserWeaponNode, bool>(storey.<>m__0));
        }

        private TutorialStepNode GetStepById(long id, ICollection<TutorialStepNode> steps)
        {
            TutorialStepNode node2;
            using (IEnumerator<TutorialStepNode> enumerator = steps.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        TutorialStepNode current = enumerator.Current;
                        if (current.tutorialStepData.StepId != id)
                        {
                            continue;
                        }
                        node2 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return node2;
        }

        private TutorialNode GetTutorialById(long id, ICollection<TutorialNode> tutorials)
        {
            TutorialNode node2;
            using (IEnumerator<TutorialNode> enumerator = tutorials.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        TutorialNode current = enumerator.Current;
                        if (current.tutorialData.TutorialId != id)
                        {
                            continue;
                        }
                        node2 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return node2;
        }

        [OnEventFire]
        public void LogTutorialStep(ShowTutorialStepEvent e, TutorialStepNode step, [JoinByTutorial] TutorialNode tutorial, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            base.ScheduleEvent(new TutorialActionEvent(tutorial.tutorialData.TutorialId, step.tutorialStepData.StepId, TutorialAction.START), session);
        }

        [OnEventFire]
        public void ModuleResearchAvailable(CheckModuleForResearchEvent e, Node any, [JoinAll] ICollection<MarketModuleNode> modules)
        {
            foreach (MarketModuleNode node in modules)
            {
                if (node.marketItemGroup.Key == e.ItemId)
                {
                    if (base.Select<UserModuleNode>(node.Entity, typeof(MarketItemGroupComponent)).Count <= 0)
                    {
                        IList<ModuleCardNode> list2 = base.Select<ModuleCardNode>(node.Entity, typeof(ParentGroupComponent));
                        if (list2.Count > 0)
                        {
                            ModulePrice craftPrice = node.moduleCardsComposition.CraftPrice;
                            if (list2[0].userItemCounter.Count >= craftPrice.Cards)
                            {
                                e.ResearchAvailable = true;
                            }
                        }
                    }
                    break;
                }
            }
        }

        [OnEventFire]
        public void ModuleUpgradeAvailable(CheckModuleForUpgradeEvent e, Node any, [JoinAll] ICollection<UserModuleNode> modules, [JoinAll] SingleNode<NewModulesScreenUIComponent> screen)
        {
            foreach (UserModuleNode node in modules)
            {
                if (node.marketItemGroup.Key == e.ItemId)
                {
                    if (node.moduleUpgradeLevel.Level == 0L)
                    {
                        IList<ModuleCardNode> list = base.Select<ModuleCardNode>(node.Entity, typeof(ParentGroupComponent));
                        if (list.Count > 0)
                        {
                            ModulePrice price = node.moduleCardsComposition.UpgradePrices[0];
                            if ((list[0].userItemCounter.Count >= price.Cards) && (screen.component.tankPartModeController.GetMode() == TankPartModuleType.WEAPON))
                            {
                                e.UpgradeAvailable = true;
                            }
                        }
                    }
                    break;
                }
            }
        }

        [OnEventFire]
        public void OpenTutorialContainer(OpenTutorialContainerEvent e, Node any)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(e.ItemId);
            if (item != null)
            {
                IList<GamePlayChestItemNode> source = base.Select<GamePlayChestItemNode>(item.MarketItem, typeof(MarketItemGroupComponent));
                if (source.Count > 0)
                {
                    base.ScheduleEvent(new OpenContainerEvent(), source.Single<GamePlayChestItemNode>().Entity);
                }
            }
        }

        [OnEventFire]
        public void OpenTutorialContainerDialog(OpenTutorialContainerDialogEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            TutorialContainerDialog dialog = dialogs.component.Get<TutorialContainerDialog>();
            e.dialog = dialog;
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(e.ItemId);
            if (item != null)
            {
                Entity marketItem = item.MarketItem;
                if ((marketItem != null) && marketItem.HasComponent<ImageItemComponent>())
                {
                    dialog.ConatinerImageUID = marketItem.GetComponent<ImageItemComponent>().SpriteUid;
                }
            }
            dialog.ChestCount = e.ItemsCount;
            List<Animator> animators = new List<Animator>();
            if (screens.IsPresent())
            {
                animators = screens.Get().component.Animators;
            }
            dialog.Show(animators);
        }

        [OnEventFire]
        public void PauseTutorialOnLoginReward(NodeAddedEvent e, SingleNode<LoginRewardDialog> loginRewardDialog)
        {
            if (TutorialCanvas.Instance.IsShow)
            {
                TutorialCanvas.Instance.Pause();
            }
        }

        private bool RequiredTutorialsShowed(TutorialNode tutorial, ICollection<TutorialNode> tutorials)
        {
            bool flag;
            List<long> tutorialsIds = tutorial.tutorialRequiredCompletedTutorials.TutorialsIds;
            if (tutorialsIds == null)
            {
                return true;
            }
            using (List<long>.Enumerator enumerator = tutorialsIds.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        long current = enumerator.Current;
                        TutorialNode tutorialById = this.GetTutorialById(current, tutorials);
                        if ((tutorialById == null) || tutorialById.Entity.HasComponent<TutorialCompleteComponent>())
                        {
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag;
        }

        public bool ShowTutorial(SingleNode<TutorialShowTriggerComponent> tutorialTrigger, ICollection<TutorialNode> tutorials)
        {
            base.Log.InfoFormat("Try Show Tutorial {0}", tutorialTrigger.component.StepId);
            if (tutorialTrigger.component.ShowAllow())
            {
                TutorialNode tutorialById = this.GetTutorialById(tutorialTrigger.component.TutorialId, tutorials);
                if (tutorialById == null)
                {
                    return false;
                }
                if (!this.RequiredTutorialsShowed(tutorialById, tutorials))
                {
                    return false;
                }
                if (tutorialById.Entity.HasComponent<TutorialCompleteComponent>())
                {
                    base.Log.InfoFormat("Tutorial already complete {0}", tutorialById.tutorialData.TutorialId);
                    return false;
                }
                if ((this.activeTutorial != null) && !tutorialById.Entity.Equals(this.activeTutorial.Entity))
                {
                    base.Log.Info("already have active tutorial");
                    return false;
                }
                IList<TutorialStepNode> list = base.Select<TutorialStepNode>(tutorialById.Entity, typeof(TutorialGroupComponent));
                bool flag = true;
                for (int i = 0; i < list.Count; i++)
                {
                    bool flag2 = list[i].Entity.HasComponent<TutorialStepCompleteComponent>();
                    if (flag && (!flag2 && (list[i].tutorialStepData.StepId == tutorialTrigger.component.StepId)))
                    {
                        if (this.activeTutorial == null)
                        {
                            this.activeTutorial = tutorialById;
                            base.Log.InfoFormat("Active tutorial: {0}", tutorialById.tutorialData.TutorialId);
                        }
                        base.Log.InfoFormat("Show step: {0} id: {1}", tutorialTrigger.component.name, tutorialTrigger.component.StepId);
                        base.ScheduleEvent<ShowTutorialStepEvent>(list[i].Entity);
                        TutorialStepIndexEvent eventInstance = new TutorialStepIndexEvent();
                        base.ScheduleEvent(eventInstance, list[i].Entity);
                        tutorialTrigger.component.Show(list[i].Entity, eventInstance.CurrentStepNumber, eventInstance.StepCountInTutorial);
                        return true;
                    }
                    flag = flag2;
                }
            }
            return false;
        }

        [OnEventFire]
        public void ShowTutorialOnEulaClose(NodeRemoveEvent e, SingleNode<EulaNotificationComponent> eula, [JoinAll] SingleNode<MainScreenComponent> screen)
        {
            base.NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnEulaClose(TriggerOnEulaCloseEvent e, Node any, [JoinAll, Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger)
        {
            if ((tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere) && (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger))
            {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void ShowTutorialOnLoginRewardClose(NodeRemoveEvent e, SingleNode<LoginRewardsNotificationComponent> loginRewardNotification, [JoinAll] SingleNode<MainScreenComponent> screen)
        {
            base.NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnPPClose(NodeRemoveEvent e, SingleNode<PrivacyPolicyNotificationComponent> pp, [JoinAll] SingleNode<MainScreenComponent> screen)
        {
            base.NewEvent<TriggerOnEulaCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnReleaseClose(NodeRemoveEvent e, SingleNode<ReleaseGiftsNotificationComponent> eula, [JoinAll] SingleNode<MainScreenComponent> screen)
        {
            base.NewEvent<TriggerOnReleaseCloseEvent>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowTutorialOnReleaseClose(TriggerOnReleaseCloseEvent e, Node any, [JoinAll, Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger)
        {
            if ((tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere) && (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger))
            {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void SkipActiveTutorial(CompleteActiveTutorialEvent e, Node any)
        {
            if (this.activeTutorial != null)
            {
                base.ScheduleEvent<TutorialCompleteEvent>(this.activeTutorial.Entity);
            }
        }

        [OnEventFire]
        public void SkipAllTutorials(SkipAllTutorialsEvent e, Node any, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            selfUser.Entity.AddComponent<SkipAllTutorialsComponent>();
        }

        [OnEventFire]
        public void StepAdded(NodeAddedEvent e, SingleNode<TutorialShowTriggerComponent> tutorialTrigger, [JoinAll] ICollection<TutorialNode> tutorials)
        {
            if (tutorials.Count != 0)
            {
                bool flag = false;
                bool flag2 = false;
                foreach (TutorialNode node in tutorials)
                {
                    if (node.tutorialData.TutorialId == tutorialTrigger.component.TutorialId)
                    {
                        flag = node.Entity.HasComponent<TutorialCompleteComponent>();
                        foreach (TutorialStepNode node2 in base.Select<TutorialStepNode>(node.Entity, typeof(TutorialGroupComponent)))
                        {
                            if (node2.tutorialStepData.StepId == tutorialTrigger.component.StepId)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (flag || !flag2)
                {
                    tutorialTrigger.component.DestroyTrigger();
                    base.Log.InfoFormat("TutorialShowTriggerComponent added, step {0} not found", tutorialTrigger.component.StepId);
                }
                else if ((tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhere) && (tutorialTrigger.component.triggerType != TutorialShowTriggerComponent.EventTriggerType.CustomTrigger))
                {
                    tutorialTrigger.component.Triggered();
                }
            }
        }

        [OnEventFire]
        public void StepComplete(TutorialStepCompleteEvent e, TutorialStepNode stepNode, [JoinByTutorial] ICollection<TutorialStepNode> steps, [JoinAll] ICollection<SingleNode<TutorialShowTriggerComponent>> triggers, [JoinAll] ICollection<TutorialNode> tutorials, [JoinAll] SingleNode<ClientSessionComponent> session)
        {
            if (!stepNode.Entity.HasComponent<TutorialStepCompleteComponent>())
            {
                stepNode.Entity.AddComponent<TutorialStepCompleteComponent>();
            }
            TutorialNode node = base.Select<TutorialNode>(stepNode.Entity, typeof(TutorialGroupComponent)).Single<TutorialNode>();
            base.ScheduleEvent(new TutorialActionEvent(node.tutorialData.TutorialId, stepNode.tutorialStepData.StepId, TutorialAction.END), session);
            bool flag = true;
            if (!node.Entity.HasComponent<TutorialCompleteComponent>())
            {
                foreach (TutorialStepNode node2 in steps)
                {
                    flag = node2.Entity.HasComponent<TutorialStepCompleteComponent>();
                    if (!flag)
                    {
                        break;
                    }
                }
            }
            if (flag)
            {
                this.activeTutorial = null;
                if (!node.Entity.HasComponent<TutorialCompleteComponent>())
                {
                    node.Entity.AddComponent<TutorialCompleteComponent>();
                }
                base.Log.Info("Tutorial complete, save on server");
                base.ScheduleEvent(new ApplyTutorialIdEvent(node.tutorialData.TutorialId), session);
            }
            foreach (SingleNode<TutorialShowTriggerComponent> node3 in triggers)
            {
                if (((node3.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.Awake) || (node3.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhereOrDelay)) && this.ShowTutorial(node3, tutorials))
                {
                    break;
                }
            }
        }

        [OnEventFire]
        public void TryShowTutorial(TutorialTriggerEvent e, SingleNode<TutorialShowTriggerComponent> tutorialTrigger, [JoinAll] ICollection<TutorialNode> tutorials, [JoinAll] ICollection<NotificationNode> notifications, [JoinAll] SingleNode<Dialogs60Component> dialogs60, [JoinAll] Optional<TurnOffTitorialUserNode> reservedUser, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            CheckForActiveNotificationsEvent eventInstance = new CheckForActiveNotificationsEvent();
            base.ScheduleEvent(eventInstance, tutorialTrigger);
            if (!reservedUser.IsPresent() && (!selfUser.Entity.HasComponent<SkipAllTutorialsComponent>() && !eventInstance.NotificationExist))
            {
                base.Log.InfoFormat("TryShowTutorial {0}", tutorialTrigger.component.StepId);
                if (this.ShowTutorial(tutorialTrigger, tutorials))
                {
                    tutorialTrigger.component.DestroyTrigger();
                    foreach (NotificationNode node in notifications)
                    {
                        base.ScheduleEvent<CloseNotificationEvent>(node);
                    }
                    dialogs60.component.CloseAll(tutorialTrigger.component.ignorableDialogName);
                }
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <CheckBoughtWepon>c__AnonStorey1
        {
            internal CheckBoughtItemEvent e;

            internal bool <>m__0(TutorialShowSystem.UserWeaponNode userWeapon) => 
                userWeapon.marketItemGroup.Key == this.e.ItemId;
        }

        [CompilerGenerated]
        private sealed class <GetChangeTurretTutorialValidationData>c__AnonStorey0
        {
            internal GetChangeTurretTutorialValidationDataEvent e;

            internal bool <>m__0(TutorialShowSystem.UserWeaponNode userWeapon) => 
                userWeapon.marketItemGroup.Key == this.e.ItemId;
        }

        public class ActiveTutorialNode : TutorialShowSystem.TutorialNode
        {
            public ActiveTutorialComponent activeTutorial;
        }

        public class CheckForActiveNotificationsEvent : Event
        {
            public bool NotificationExist;
        }

        public class EmailConfirmationNotificationNode : Node
        {
            public NotificationComponent notification;
            public NotificationMessageComponent notificationMessage;
            public NotificationConfigComponent notificationConfig;
            public ActiveNotificationComponent activeNotification;
            public NotifficationMappingComponent notifficationMapping;
            public EmailConfirmationNotificationComponent emailConfirmationNotification;
        }

        public class GamePlayChestItemNode : Node
        {
            public GameplayChestItemComponent gameplayChestItem;
            public ContainerMarkerComponent containerMarker;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class ItemWithImageNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public ItemIconComponent itemIcon;
        }

        public class MarketModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public MarketItemGroupComponent marketItemGroup;
            public ModuleCardsCompositionComponent moduleCardsComposition;
        }

        public class ModuleCardNode : Node
        {
            public ModuleCardItemComponent moduleCardItem;
            public UserItemComponent userItem;
            public UserItemCounterComponent userItemCounter;
        }

        public class MountedWeaponNode : TutorialShowSystem.UserWeaponNode
        {
            public MountedItemComponent mountedItem;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class NotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
        }

        public class SlotNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
        }

        public class TriggerOnReleaseCloseEvent : Event
        {
        }

        public class TurnOffTitorialUserNode : Node
        {
            public SelfUserComponent selfUser;
            public TurnOffTutorialUserComponent turnOffTutorialUser;
        }

        public class TutorialNode : Node
        {
            public TutorialDataComponent tutorialData;
            public TutorialGroupComponent tutorialGroup;
            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }

        public class TutorialStepNode : Node
        {
            public TutorialStepDataComponent tutorialStepData;
            public TutorialGroupComponent tutorialGroup;
        }

        public class UserModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public ModuleGroupComponent moduleGroup;
            public MarketItemGroupComponent marketItemGroup;
            public UserItemComponent userItem;
            public ModuleUpgradeLevelComponent moduleUpgradeLevel;
            public ModuleCardsCompositionComponent moduleCardsComposition;
        }

        public class UserWeaponNode : Node
        {
            public UserItemComponent userItem;
            public WeaponItemComponent weaponItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

