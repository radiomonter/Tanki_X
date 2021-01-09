namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class ProfileScreenNavigationSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateContextAndShowProfileScreen(ShowProfileScreenEvent e, Node any, [JoinAll] SelfUserNode selfUser)
        {
            Entity context = base.CreateEntity("ProfileScreenContext_userId : " + e.UserId);
            context.AddComponent(new ProfileScreenContextComponent(e.UserId));
            ShowScreenDownEvent<ProfileScreenComponent> eventInstance = new ShowScreenDownEvent<ProfileScreenComponent>();
            eventInstance.SetContext(context, true);
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
        }

        [OnEventFire]
        public void SendShowProfileScreenEvent(ButtonClickEvent e, SingleNode<ProfileButtonComponent> node, [JoinAll] SelfUserNode selfUser)
        {
            base.ScheduleEvent(new ShowProfileScreenEvent(selfUser.Entity.Id), EngineService.EntityStub);
        }

        [OnEventFire]
        public void SendShowProfileScreenEvent(UserLabelAvatarClickEvent e, SingleNode<UserLabelComponent> userLabel, [JoinByUser] SomeUserNode someUser)
        {
            base.ScheduleEvent(new ShowProfileScreenEvent(someUser.Entity.Id), EngineService.EntityStub);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Not(typeof(UserIncompleteRegistrationComponent))]
        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
        }

        [Not(typeof(UserIncompleteRegistrationComponent))]
        public class SomeUserNode : Node
        {
            public UserComponent user;
        }
    }
}

