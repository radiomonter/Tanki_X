namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class TopPanelSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateTopPanelItems(NodeAddedEvent e, ActiveScreenNode screen, SingleNode<CommonScreenElementsComponent> topPanel)
        {
            topPanel.component.ActivateItems(screen.screen.VisibleCommonScreenElements);
        }

        [OnEventFire]
        public void DisableBackButtonWhenLoad(EnterBattleAttemptEvent e, Node anyNode, [JoinAll] SingleNode<BackButtonComponent> backButton)
        {
            backButton.component.Disabled = true;
        }

        [OnEventFire]
        public void EnableBackButtonWhenLoadFail(EnterBattleFailedEvent e, Node anyNode, [JoinAll] SingleNode<BackButtonComponent> backButton)
        {
            backButton.component.Disabled = false;
        }

        [OnEventFire]
        public void GoBack(ButtonClickEvent e, SingleNode<BackButtonComponent> button)
        {
            if (!button.component.Disabled)
            {
                base.ScheduleEvent<GoBackRequestEvent>(button.Entity);
            }
        }

        [OnEventComplete]
        public void SendHeaderTextEvent(NodeAddedEvent e, TopPanelNode topPanel, SingleNode<ScreenHeaderTextComponent> screenHeader, [Context, JoinByScreen] SingleNode<ActiveScreenComponent> screen)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent();
            eventInstance.Animated(screenHeader.component.HeaderText);
            base.ScheduleEvent(eventInstance, screenHeader.Entity);
        }

        [OnEventFire]
        public void SetHeaderText(SetScreenHeaderEvent e, Node any, [JoinAll] TopPanelNode topPanel)
        {
            if (e.Animate)
            {
                topPanel.topPanel.SetHeaderText(e.Header);
            }
            else
            {
                topPanel.topPanel.SetHeaderTextImmediately(e.Header);
            }
        }

        [OnEventFire]
        public void ShowHeaderAnimation(GoBackEvent e, Node any, [JoinAll] TopPanelNode topPanel)
        {
            topPanel.topPanel.screenHeader.SetTrigger("back");
        }

        [OnEventFire]
        public void ShowHeaderAnimation(ShowScreenEvent e, Node any, [JoinAll] TopPanelNode topPanel)
        {
            if (topPanel.topPanel.HasHeader)
            {
                topPanel.topPanel.screenHeader.SetTrigger("forward");
            }
        }

        [OnEventFire]
        public void UpdateBackgroundVisibility(NodeAddedEvent e, ScreenWithTopPanelConstructorNode screen, TopPanelNode topPanel)
        {
            topPanel.topPanel.background.gameObject.SetActive(screen.topPanelConstructor.ShowBackground);
        }

        [OnEventFire]
        public void UpdateHeaderVisibility(NodeAddedEvent e, ScreenWithTopPanelConstructorNode screen, TopPanelNode topPanel)
        {
            topPanel.topPanel.screenHeader.gameObject.SetActive(screen.topPanelConstructor.ShowHeader);
        }

        public class ActiveScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }

        public class NavigationNode : Node
        {
            public CurrentScreenComponent currentScreen;
        }

        public class ScreenWithTopPanelConstructorNode : Node
        {
            public ScreenComponent screen;
            public TopPanelConstructorComponent topPanelConstructor;
        }

        public class TopPanelNode : Node
        {
            public TopPanelComponent topPanel;
        }
    }
}

