namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class CodeInputValidationSystem : ECSSystem
    {
        [OnEventFire]
        public void SwitchInputToNormalState(InputFieldValueChangedEvent e, CodeInputFieldInvalidStateNode inputField)
        {
            inputField.esm.Esm.ChangeState<InputFieldStates.NormalState>();
        }

        [OnEventFire]
        public void SwitchToAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldNotAcceptableNode inputField)
        {
            if (inputField.inputField.Input.Length > 0)
            {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(NodeAddedEvent e, InvalidStateNode inputField)
        {
            inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
        }

        [OnEventFire]
        public void SwitchToNotAcceptableState(InputFieldValueChangedEvent e, CodeInputFieldAcceptableNode inputField)
        {
            if (inputField.inputField.Input.Length == 0)
            {
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        public class CodeInputFieldAcceptableNode : Node
        {
            public CodeInputFieldComponent codeInputField;
            public InputFieldComponent inputField;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public AcceptableStateComponent acceptableState;
        }

        public class CodeInputFieldInvalidStateNode : Node
        {
            public CodeInputFieldComponent codeInputField;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public ESMComponent esm;
        }

        public class CodeInputFieldNode : Node
        {
            public CodeInputFieldComponent codeInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
        }

        public class CodeInputFieldNotAcceptableNode : Node
        {
            public CodeInputFieldComponent codeInputField;
            public InputFieldComponent inputField;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
            public NotAcceptableStateComponent notAcceptableState;
        }

        public class InvalidStateNode : Node
        {
            public CodeInputFieldComponent codeInputField;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }
    }
}

