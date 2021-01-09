namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class BackgroundSystem : ECSSystem
    {
        [OnEventFire]
        public void HideBackground(NodeAddedEvent e, HidingScreenNode screen, [JoinAll] SingleNode<BackgroundComponent> background)
        {
            background.component.Hide();
        }

        [OnEventFire]
        public void MoveBackgroundOnInit(NodeAddedEvent e, SingleNode<BackgroundComponent> background, SingleNode<ScreensLayerComponent> screensLayer)
        {
            background.component.transform.SetParent(screensLayer.component.transform, false);
            background.component.transform.SetAsFirstSibling();
        }

        [OnEventFire]
        public void ShowBackground(NodeAddedEvent e, ActiveScreenNode screen, [JoinAll] SingleNode<BackgroundComponent> background)
        {
            background.component.Show();
        }

        public class ActiveScreenNode : Node
        {
            public ShowBackgroundComponent showBackground;
            public ActiveScreenComponent activeScreen;
        }

        public class HidingScreenNode : Node
        {
            public ShowBackgroundComponent showBackground;
            public ScreenHidingComponent screenHiding;
        }
    }
}

