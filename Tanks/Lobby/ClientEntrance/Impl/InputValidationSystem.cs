namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public abstract class InputValidationSystem : ECSSystem
    {
        protected InputValidationSystem()
        {
        }

        public class BaseInputFieldNode<TMarker> : Node where TMarker: Component
        {
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public ScreenGroupComponent screenGroup;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public TMarker marker;

            public void ToAwaitState()
            {
                this.ToState<InteractivityPrerequisiteStates.NotAcceptableState, InputFieldStates.AwaitState>();
            }

            public void ToInvalidState(string errorMessage)
            {
                this.ToState<InteractivityPrerequisiteStates.NotAcceptableState, InputFieldStates.InvalidState>();
                this.inputField.ErrorMessage = errorMessage;
            }

            public void ToNormalState()
            {
                this.ToState<InteractivityPrerequisiteStates.NotAcceptableState, InputFieldStates.NormalState>();
            }

            private void ToState<TPrerequisiteState, TInputFieldState>() where TPrerequisiteState: Node where TInputFieldState: Node
            {
                if (((this.inputField.InputField != null) && this.inputField.InputField.IsInteractable()) || ((this.inputField.TMPInputField != null) && this.inputField.TMPInputField.IsInteractable()))
                {
                    this.interactivityPrerequisiteESM.Esm.ChangeState<TPrerequisiteState>();
                    this.esm.Esm.ChangeState<TInputFieldState>();
                }
            }

            public void ToValidState()
            {
                this.ToState<InteractivityPrerequisiteStates.AcceptableState, InputFieldStates.ValidState>();
            }

            public string Input =>
                this.inputField.Input;
        }
    }
}

