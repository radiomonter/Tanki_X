namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class LinkNavigationSystem : ECSSystem
    {
        private bool IsParsed(ParseLinkEvent e) => 
            (e.CustomNavigationEvent != null) || !ReferenceEquals(e.ScreenType, null);

        [OnEventFire]
        public void NavigateLink(ButtonClickEvent e, SingleNode<LinkButtonComponent> button)
        {
            NavigateLinkEvent eventInstance = new NavigateLinkEvent {
                Link = button.component.Link
            };
            base.ScheduleEvent(eventInstance, button);
        }

        [OnEventFire]
        public void NavigateLink(NavigateLinkEvent e, Node node)
        {
            ParseLinkEvent eventInstance = new ParseLinkEvent {
                Link = e.Link
            };
            base.ScheduleEvent(eventInstance, node);
            if (!this.IsParsed(eventInstance))
            {
                if (eventInstance.ParseMessage != null)
                {
                    base.Log.ErrorFormat("Link parse error: {0}, ParseMessage: {1}", e.Link, eventInstance.ParseMessage);
                }
                else
                {
                    base.Log.ErrorFormat("Link parse error: {0}", e.Link);
                }
            }
            else if (eventInstance.CustomNavigationEvent != null)
            {
                eventInstance.CustomNavigationEvent.Schedule();
            }
            else
            {
                ShowScreenEvent event4 = new ShowScreenEvent(eventInstance.ScreenType, AnimationDirection.LEFT);
                if (eventInstance.ScreenContext != null)
                {
                    event4.SetContext(eventInstance.ScreenContext, eventInstance.ScreenContextAutoDelete);
                }
                base.ScheduleEvent(event4, node);
            }
        }
    }
}

