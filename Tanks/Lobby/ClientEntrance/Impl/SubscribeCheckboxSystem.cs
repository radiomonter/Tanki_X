namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class SubscribeCheckboxSystem : ECSSystem
    {
        [OnEventFire]
        public void InitSubscribeCheckbox(NodeAddedEvent e, SubscribeCheckboxNode subscribeCheckbox, [JoinAll] Optional<UserNode> user)
        {
            subscribeCheckbox.checkbox.IsChecked = !user.IsPresent() || user.Get().userSubscribe.Subscribed;
        }

        [OnEventFire]
        public void SendSubscribeToServer(CheckboxEvent e, SubscribeCheckboxNode checkbox, [JoinAll] UserNode user, [JoinAll] Optional<SingleNode<RegistrationScreenComponent>> registration)
        {
            bool isChecked = e.IsChecked;
            if (!registration.IsPresent() && (user.userSubscribe.Subscribed != isChecked))
            {
                SubscribeChangeEvent eventInstance = new SubscribeChangeEvent {
                    Subscribed = isChecked
                };
                base.ScheduleEvent(eventInstance, user.Entity);
            }
        }

        public class SubscribeCheckboxNode : Node
        {
            public SubscribeCheckboxComponent subscribeCheckbox;
            public CheckboxComponent checkbox;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserSubscribeComponent userSubscribe;
        }
    }
}

