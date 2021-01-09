namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientSettings.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class DailyBonusScreenSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<SingleNode<ContainersUI>> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<SingleNode<ContainersUI>> <>f__am$cache1;

        [OnEventFire]
        public void ChangeCycle(DailyBonusCycleSwitchedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode)
        {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void ChangeZone(DailyBonusZoneSwitchedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode)
        {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void CloseDailyBonusDialog(ButtonClickEvent e, SingleNode<CloseDailyBonusDialogButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            dialogs.component.Get<DailyBonusScreenComponent>().Hide();
        }

        [OnEventFire]
        public void GoToDailyBonusScreen(ButtonClickEvent e, SingleNode<DailyBonusMainScreenButtonComponent> button, [JoinAll] UserDailyBonusNode user, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if ((user.userDailyBonusReceivedRewards.ReceivedRewards.Count == 0) && (user.userDailyBonusCycle.CycleNumber == 0L))
            {
                dialogs.component.Get<DailyBonusInfoScreen>().Show();
            }
            else
            {
                dialogs.component.Get<DailyBonusScreenComponent>().Show();
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] UserDailyBonusNode userDailyBonusNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode)
        {
            screenNode.component.UpdateViewInNextFrame(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void InitSounds(NodeAddedEvent e, SoundListenerNode listener, UserReadyForLobbyNode user)
        {
            if (UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>() == null)
            {
                UISoundEffectController.UITransformRoot.gameObject.AddComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour = Object.Instantiate<DailyBonusSoundsBehaviour>(listener.soundListenerResources.Resources.DailyBonusSounds, UISoundEffectController.UITransformRoot.position, UISoundEffectController.UITransformRoot.rotation, UISoundEffectController.UITransformRoot);
            }
        }

        [OnEventFire]
        public void OnGetNewTeleportButtonClick(ButtonClickEvent e, SingleNode<GetNewTeleportButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen, [JoinAll] UserDailyBonusNode userDailyBonusNode)
        {
            base.ScheduleEvent<SwitchDailyBonusCycleEvent>(userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayUpgrade();
        }

        private void OnTakeBonusButtonClick(SingleNode<DailyBonusScreenComponent> screen, UserDailyBonusNode userDailyBonusNode)
        {
            <OnTakeBonusButtonClick>c__AnonStorey0 storey = new <OnTakeBonusButtonClick>c__AnonStorey0 {
                userDailyBonusNode = userDailyBonusNode,
                $this = this,
                selectedBonusElement = screen.component.mapView.selectedBonusElement
            };
            if (storey.selectedBonusElement == null)
            {
                throw new Exception("Tried to take a bonus when selected bonus is null");
            }
            screen.component.teleportView.activeTeleportView.GetComponent<AnimationEventListener>().SetPartyHandler(new Action(storey.<>m__0));
            screen.component.teleportView.activeTeleportView.GetComponent<Animator>().SetTrigger("party");
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayTake();
        }

        [OnEventFire]
        public void OnTakeBonusButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen, [JoinAll] UserDailyBonusNode userDailyBonusNode)
        {
            this.OnTakeBonusButtonClick(screen, userDailyBonusNode);
        }

        [OnEventFire]
        public void OnTakeBonusButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusContainerButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen, [JoinAll] UserDailyBonusNode userDailyBonusNode)
        {
            this.OnTakeBonusButtonClick(screen, userDailyBonusNode);
        }

        [OnEventFire]
        public void OnTakeDetailTargetButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusDetailTargetButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen, [JoinAll] UserDailyBonusNode userDailyBonusNode)
        {
            ReceiveTargetItemFromDetailsByDailyBonusEvent eventInstance = new ReceiveTargetItemFromDetailsByDailyBonusEvent {
                DetailMarketItemId = screen.component.completeDetailGarageItem.MarketItemId
            };
            base.ScheduleEvent(eventInstance, userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayClick();
        }

        [OnEventFire]
        public void OnUpgradeTeleportButtonClick(ButtonClickEvent e, SingleNode<UpgradeTeleportButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen, [JoinAll] UserDailyBonusNode userDailyBonusNode)
        {
            base.ScheduleEvent<SwitchDailyBonusZoneEvent>(userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayUpgrade();
        }

        [OnEventFire]
        public void RemoveSounds(NodeAddedEvent e, UserReadyToBattleNode user)
        {
            DailyBonusScreenSoundsRoot component = UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>();
            if (component != null)
            {
                Object.DestroyObject(component.gameObject);
                Object.Destroy(component);
            }
        }

        [OnEventFire]
        public void ResetBtnState(NodeAddedEvent e, DailyBonusMainScreenButtonNode button, UserDailyBonusNode user, DailyBonusConfig dailyBonusConfig)
        {
            button.dailyBonusMainScreenButton.ResetState();
        }

        [OnEventFire]
        public void UpdateBtnState(UpdateEvent e, DailyBonusMainScreenButtonNode button, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig)
        {
            this.UpdateDailyBonus(button, user, dailyBonusConfig);
        }

        [OnEventFire]
        public void UpdateContainers(TargetItemFromDailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c.component.UpdateContainerUI();
            }
            containersUis.ForEach<SingleNode<ContainersUI>>(<>f__am$cache0);
            screenNode.component.Hide();
            ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                Category = GarageCategory.CONTAINERS,
                SelectedItem = (GarageItemsRegistry.GetItem<DetailItem>(e.DetailMarketItemId).TargetMarketItem as ContainerBoxItem).MarketItem
            };
            base.ScheduleEvent(eventInstance, userDailyBonusNode);
        }

        [OnEventFire]
        public void UpdateContainers(DailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode, [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis)
        {
            <UpdateContainers>c__AnonStorey1 storey = new <UpdateContainers>c__AnonStorey1 {
                e = e
            };
            DailyBonusData data = screenNode.component.GetCycle(userDailyBonusNode, dailyBonusConfigNode).DailyBonuses.Where<DailyBonusData>(new Func<DailyBonusData, bool>(storey.<>m__0)).First<DailyBonusData>();
            if (data.ContainerReward != null)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = c => c.component.UpdateContainerUI();
                }
                containersUis.ForEach<SingleNode<ContainersUI>>(<>f__am$cache1);
                screenNode.component.Hide();
                ContainerBoxItem item = GarageItemsRegistry.GetItem<ContainerBoxItem>(data.ContainerReward.MarketItemId);
                ShowGarageCategoryEvent eventInstance = new ShowGarageCategoryEvent {
                    Category = !item.IsBlueprint ? GarageCategory.CONTAINERS : GarageCategory.BLUEPRINTS,
                    SelectedItem = item.MarketItem
                };
                this.ScheduleEvent(eventInstance, userDailyBonusNode);
            }
        }

        private void UpdateDailyBonus(DailyBonusMainScreenButtonNode button, UserDailyBonusNode user, DailyBonusConfig dailyBonusConfig)
        {
            bool flag = Date.Now >= user.userDailyBonusNextReceivingDate.Date;
            bool flag2 = false;
            if (user.userStatistics.Statistics.ContainsKey("BATTLES_PARTICIPATED"))
            {
                flag2 = user.userStatistics.Statistics["BATTLES_PARTICIPATED"] >= dailyBonusConfig.dailyBonusCommonConfig.BattleCountToUnlockDailyBonuses;
            }
            button.dailyBonusMainScreenButton.IsActiveState = flag && flag2;
            button.dailyBonusMainScreenButton.Interactable = flag2;
        }

        [OnEventFire]
        public void UpdateView(DailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode)
        {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void UpdateView(TargetItemFromDailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] DailyBonusConfig dailyBonusConfigNode)
        {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <OnTakeBonusButtonClick>c__AnonStorey0
        {
            internal MapViewBonusElement selectedBonusElement;
            internal DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode;
            internal DailyBonusScreenSystem $this;

            internal void <>m__0()
            {
                ReceiveDailyBonusEvent eventInstance = new ReceiveDailyBonusEvent {
                    Code = this.selectedBonusElement.dailyBonusData.Code
                };
                this.$this.ScheduleEvent(eventInstance, this.userDailyBonusNode);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateContainers>c__AnonStorey1
        {
            internal DailyBonusReceivedEvent e;

            internal bool <>m__0(DailyBonusData d) => 
                d.Code == this.e.Code;
        }

        public class DailyBonusConfig : Node
        {
            public DailyBonusCommonConfigComponent dailyBonusCommonConfig;
            public DailyBonusFirstCycleComponent dailyBonusFirstCycle;
            public DailyBonusEndlessCycleComponent dailyBonusEndlessCycle;
        }

        public class DailyBonusMainScreenButtonNode : Node
        {
            public DailyBonusMainScreenButtonComponent dailyBonusMainScreenButton;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerResourcesComponent soundListenerResources;
        }

        public class UserDailyBonusNode : Node
        {
            public UserDailyBonusInitializedComponent userDailyBonusInitialized;
            public UserDailyBonusCycleComponent userDailyBonusCycle;
            public UserDailyBonusReceivedRewardsComponent userDailyBonusReceivedRewards;
            public UserDailyBonusZoneComponent userDailyBonusZone;
            public UserDailyBonusNextReceivingDateComponent userDailyBonusNextReceivingDate;
            public UserStatisticsComponent userStatistics;
        }

        public class UserReadyForLobbyNode : DailyBonusScreenSystem.SelfUserNode
        {
            public UserReadyForLobbyComponent userReadyForLobby;
        }

        public class UserReadyToBattleNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}

