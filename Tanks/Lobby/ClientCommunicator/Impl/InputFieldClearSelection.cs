namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public class InputFieldClearSelection : MonoBehaviour
    {
        private InputFieldComponent inputField;
        private bool needClear;

        private void Awake()
        {
            this.inputField = base.gameObject.GetComponent<InputFieldComponent>();
        }

        private void LateUpdate()
        {
            if (this.needClear)
            {
                this.inputField.InputField.selectionAnchorPosition = this.inputField.InputField.text.Length;
                this.inputField.InputField.selectionFocusPosition = this.inputField.InputField.text.Length;
                this.needClear = false;
            }
        }

        public void OnSelect()
        {
            this.needClear = true;
        }
    }
}

