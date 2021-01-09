namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class InputFieldSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearInputOnShow(NodeAddedEvent e, ClearInputFieldNode inputField)
        {
            inputField.inputField.Input = string.Empty;
        }

        [OnEventFire]
        public void HandleAwaitState(NodeAddedEvent e, InputFieldAwaitNode node)
        {
            node.inputField.Animator.SetTrigger("Await");
        }

        [OnEventFire]
        public void HandleInvalidState(NodeAddedEvent e, InputFieldInvalidNode node)
        {
            node.inputField.Animator.SetTrigger("Invalid");
        }

        [OnEventFire]
        public void HandleNormalState(NodeAddedEvent e, InputFieldNormalNode node)
        {
            node.inputField.Animator.SetTrigger("Reset");
        }

        [OnEventFire]
        public void HandleValidState(NodeAddedEvent e, InputFieldValidNode node)
        {
            node.inputField.Animator.SetTrigger("Valid");
        }

        [OnEventFire]
        public void InitESM(NodeAddedEvent e, SingleNode<InputFieldComponent> node)
        {
            ESMComponent component = new ESMComponent();
            node.Entity.AddComponent(component);
            EntityStateMachine esm = component.Esm;
            esm.AddState<InputFieldStates.NormalState>();
            esm.AddState<InputFieldStates.InvalidState>();
            esm.AddState<InputFieldStates.ValidState>();
            esm.AddState<InputFieldStates.AwaitState>();
            esm.ChangeState<InputFieldStates.NormalState>();
        }

        public class ClearInputFieldNode : Node
        {
            public ClearInputOnShowComponent clearInputOnShow;
            public InputFieldComponent inputField;
        }

        public class InputFieldAwaitNode : Node
        {
            public InputFieldAwaitStateComponent inputFieldAwaitState;
            public InputFieldComponent inputField;
        }

        public class InputFieldInvalidNode : Node
        {
            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public InputFieldComponent inputField;
        }

        public class InputFieldNormalNode : Node
        {
            public InputFieldNormalStateComponent inputFieldNormalState;
            public InputFieldComponent inputField;
        }

        public class InputFieldValidNode : Node
        {
            public InputFieldValidStateComponent inputFieldValidState;
            public InputFieldComponent inputField;
        }
    }
}

