namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class ReturnToBattleSystem : ECSSystem
    {
        private const float TIMEOUT = 0.05f;

        [OnEventFire]
        public void DeleteEmailConfirmationNotification(NodeAddedEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] SelfUserWithReservNode user)
        {
            base.DeleteEntity(notification.Entity);
        }

        [OnEventFire]
        public void HideReturnToBattleDialog(NodeRemoveEvent e, SelfUserWithReservNode user, [JoinAll] DialogNode dialog, [JoinAll] HomeScreenNode homeScreen)
        {
            dialog.modalConfirmWindow.Hide();
        }

        private string InsertLeftTime(string template, int time) => 
            template.Replace("[LeftTime]", time.ToString());

        [OnEventFire]
        public void OnReturnFailed(ReturnToBattleFiledEvent e, SingleNode<SelfUserComponent> user)
        {
        }

        [OnEventFire]
        public void ReleaseReservation(DialogDeclineEvent e, SingleNode<ReturnToBattleDialogComponent> dialog, [JoinAll] SelfUserWithReservNode user)
        {
            base.ScheduleEvent<ReleaseReservationInBattleEvent>(user);
        }

        [OnEventFire]
        public void ReturnToBattle(DialogConfirmEvent e, DialogNode dialog, [JoinAll] SelfUserWithReservNode user, [JoinAll] SingleNode<ActiveScreenComponent> screen)
        {
            screen.Entity.AddComponent<LockedScreenComponent>();
            base.ScheduleEvent<ReturnToBattleEvent>(user);
        }

        [OnEventFire]
        public void ShowReturnToBattleDialog(TryShowDialog e, SelfUserWithReservNode user, [JoinAll] HomeScreenNode homeScreen, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if (!Flow.Current.EntityRegistry.ContainsEntity(user.reservedInBattleInfo.Map))
            {
                base.Log.ErrorFormat("map={0} not found on reservation in battle for user={1}", user.reservedInBattleInfo.Map, user);
            }
            else if (!TutorialCanvas.Instance.IsShow)
            {
                string name = Flow.Current.EntityRegistry.GetEntity(user.reservedInBattleInfo.Map).GetComponent<DescriptionItemComponent>().Name;
                ReturnToBattleDialogComponent component = dialogs.component.Get<ReturnToBattleDialogComponent>();
                ModalConfirmWindowComponent component2 = component.gameObject.GetComponent<ModalConfirmWindowComponent>();
                component2.Show(homeScreen.Entity);
                component.PreformatedMainText = string.Format(component2.MainText, name, user.reservedInBattleInfo.BattleMode);
                component2.MainText = this.InsertLeftTime(component.PreformatedMainText, (int) (user.reservedInBattleInfo.ExitTime - Date.Now));
            }
        }

        [OnEventFire]
        public void TryResumeTutorial(TryResumeTutorialEvent e, Node any, [JoinAll, Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger)
        {
            if ((tutorialTrigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.Awake) || (tutorialTrigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhereOrDelay))
            {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void TryResumeTutorialOnReleaseReservation(ReleaseReservationInBattleEvent e, SelfUserWithReservNode user, [JoinAll, Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger)
        {
            if (user.Entity.HasComponent<TurnOffTutorialUserComponent>())
            {
                user.Entity.RemoveComponent<TurnOffTutorialUserComponent>();
            }
            base.NewEvent<TryResumeTutorialEvent>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventFire]
        public void TryShowReturnToBattleDialog(NodeAddedEvent e, HomeScreenNode homeScreen, [JoinAll] SelfUserWithReservNode user)
        {
            base.NewEvent<TryShowDialog>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventFire]
        public void TurnOffTutorial(NodeAddedEvent e, SelfUserWithReservNode user)
        {
            if (!user.Entity.HasComponent<TurnOffTutorialUserComponent>())
            {
                user.Entity.AddComponent<TurnOffTutorialUserComponent>();
            }
        }

        [OnEventFire]
        public void TurnOnTutorial(NodeRemoveEvent e, SelfUserWithReservNode user)
        {
            if (user.Entity.HasComponent<TurnOffTutorialUserComponent>())
            {
                user.Entity.RemoveComponent<TurnOffTutorialUserComponent>();
            }
            if (!user.Entity.HasComponent<BattleGroupComponent>())
            {
                base.NewEvent<TryResumeTutorialEvent>().Attach(user).ScheduleDelayed(0.05f);
            }
        }

        [OnEventFire]
        public void UpdateTimer(UpdateEvent e, DialogNode dialog, [JoinAll] HomeScreenNode homeScreen, [JoinAll] SelfUserWithReservNode user)
        {
            int time = (int) (user.reservedInBattleInfo.ExitTime - Date.Now);
            if (dialog.returnToBattleDialog.SecondsLeft != time)
            {
                dialog.returnToBattleDialog.SecondsLeft = time;
                dialog.modalConfirmWindow.MainText = this.InsertLeftTime(dialog.returnToBattleDialog.PreformatedMainText, time);
                if (dialog.returnToBattleDialog.SecondsLeft <= 0)
                {
                    dialog.modalConfirmWindow.Hide();
                    base.ScheduleEvent<ReleaseReservationInBattleEvent>(user);
                }
            }
        }

        public class DialogNode : Node
        {
            public ModalConfirmWindowComponent modalConfirmWindow;
            public ReturnToBattleDialogComponent returnToBattleDialog;
        }

        public class HomeScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
            public MainScreenComponent mainScreen;
        }

        public class SelfUserWithReservNode : Node
        {
            public SelfUserComponent selfUser;
            public ReservedInBattleInfoComponent reservedInBattleInfo;
        }

        public class TryResumeTutorialEvent : Event
        {
        }

        public class TryShowDialog : Event
        {
        }
    }
}

