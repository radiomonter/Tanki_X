namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class ViewUserEmailScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void EmailChanged(ConfirmedUserEmailChangedEvent e, SelfUserWithConfirmedEmailNode user, [JoinAll] SingleNode<ViewUserEmailScreenComponent> screen)
        {
            this.SetEmail(user.confirmedUserEmail, screen.component);
        }

        private void SetEmail(ConfirmedUserEmailComponent userEmail, ViewUserEmailScreenComponent screen)
        {
            screen.YourEmailReplaced = screen.YourEmail.Replace("%EMAIL%", string.Format("<color=#{1}>{0}</color>", userEmail.Email, screen.EmailColor.ToHexString()));
        }

        [OnEventFire]
        public void ViewEmail(NodeAddedEvent e, SingleNode<ViewUserEmailScreenComponent> screen, SelfUserWithConfirmedEmailNode user)
        {
            this.SetEmail(user.confirmedUserEmail, screen.component);
        }

        public class SelfUserWithConfirmedEmailNode : Node
        {
            public ConfirmedUserEmailComponent confirmedUserEmail;
            public SelfUserComponent selfUser;
        }
    }
}

