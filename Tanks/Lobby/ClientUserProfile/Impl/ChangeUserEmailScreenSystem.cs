namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class ChangeUserEmailScreenSystem : ECSSystem
    {
        [OnEventComplete]
        public void HideHint(NodeAddedEvent e, SingleNode<ChangeUserEmailScreenComponent> screen, SelfUserWithConfirmedEmailNode user)
        {
            screen.component.DeactivateHint();
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<ConfirmUserEmailScreenComponent>>(screen);
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen, [JoinByScreen] EmailInputNode emailInput)
        {
            if (screen.Entity.HasComponent<LockedScreenComponent>())
            {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedChangeUserEmailScreenNode screen, [JoinByScreen] EmailInputNode emailInput)
        {
            if (screen.Entity.HasComponent<LockedScreenComponent>())
            {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }
            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        public class EmailInputNode : Node
        {
            public EmailInputFieldComponent emailInputField;
            public InputFieldComponent inputField;
            public InputFieldValidStateComponent inputFieldValidState;
            public ESMComponent esm;
        }

        public class LockedChangeUserEmailScreenNode : Node
        {
            public ChangeUserEmailScreenComponent changeUserEmailScreen;
            public LockedScreenComponent lockedScreen;
        }

        public class SelfUserWithConfirmedEmailNode : Node
        {
            public ConfirmedUserEmailComponent confirmedUserEmail;
            public SelfUserComponent selfUser;
        }
    }
}

