namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class InputAction : MonoBehaviour
    {
        [SerializeField]
        public InputActionContextId contextId;
        [SerializeField]
        public InputActionId actionId;
        public KeyCode[] keys;
        public MultiKeys[] multiKeys;
        public UnityInputAxes[] axes;
        public bool onlyPositiveAxes;
        public bool onlyNegativeAxes;
        public bool invertAxes;
        protected bool activated;
        protected Action startHandler;
        protected Action stopHandler;

        public static implicit operator bool(InputAction action) => 
            (action != null) && action.activated;

        public void StartInputAction()
        {
            this.activated = true;
            if (this.startHandler != null)
            {
                this.startHandler();
            }
        }

        public void StopInputAction()
        {
            this.activated = false;
            if (this.stopHandler != null)
            {
                this.stopHandler();
            }
        }

        public override string ToString() => 
            $"[InputAction: name={this.actionId.actionName}, keys={this.keys}]";

        public bool Activated =>
            this.activated;

        public Action StartHandler
        {
            get => 
                this.startHandler;
            set => 
                this.startHandler = value;
        }

        public Action StopHandler
        {
            get => 
                this.stopHandler;
            set => 
                this.stopHandler = value;
        }
    }
}

