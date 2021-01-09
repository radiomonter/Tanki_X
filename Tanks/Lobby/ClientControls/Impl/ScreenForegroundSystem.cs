namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class ScreenForegroundSystem : ECSSystem
    {
        [OnEventFire]
        public void ForceHideScreenForeground(ForceHideScreenForegroundEvent e, ForegroundNode foreground)
        {
            foreground.screenForeground.Count = 0;
            HideForeground(foreground);
        }

        private static void HideForeground(ForegroundNode foreground)
        {
            foreground.screenForegroundAnimation.Animator.SetBool("visible", false);
        }

        [OnEventFire]
        public void HideScreenForeground(HideScreenForegroundEvent e, ForegroundNode foreground)
        {
            if (foreground.screenForeground.Count > 0)
            {
                foreground.screenForeground.Count--;
            }
            if (foreground.screenForeground.Count == 0)
            {
                HideForeground(foreground);
            }
        }

        [OnEventFire]
        public void SendHideScreenEvent(NodeRemoveEvent e, SingleNode<ShowScreenForegroundComponent> screen, [JoinAll] ForegroundNode foreground)
        {
            base.ScheduleEvent<HideScreenForegroundEvent>(foreground);
        }

        [OnEventFire]
        public void SendShowScreenEvent(NodeAddedEvent e, SingleNode<ShowScreenForegroundComponent> screen, [JoinAll] ForegroundNode foreground)
        {
            ShowScreenForegroundEvent eventInstance = new ShowScreenForegroundEvent {
                Alpha = screen.component.Alpha
            };
            base.ScheduleEvent(eventInstance, foreground);
        }

        [OnEventFire]
        public void ShowScreenForeground(ShowScreenForegroundEvent e, ForegroundNode foreground)
        {
            foreground.screenForeground.Count++;
            ScreenForegroundAnimationComponent screenForegroundAnimation = foreground.screenForegroundAnimation;
            screenForegroundAnimation.Animator.SetBool("visible", true);
            screenForegroundAnimation.Alpha = e.Alpha;
        }

        public class ForegroundNode : Node
        {
            public ScreenForegroundComponent screenForeground;
            public ScreenForegroundAnimationComponent screenForegroundAnimation;
        }
    }
}

