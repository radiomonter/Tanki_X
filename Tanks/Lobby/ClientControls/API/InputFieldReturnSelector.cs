namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class InputFieldReturnSelector : MonoBehaviour
    {
        public Selectable selectable;
        public InputField inputField;
        public InputFieldReturnSelector overridenSelector;

        public bool CanNavigateToSelectable() => 
            this.selectable.interactable;

        protected virtual bool CurrentInputSelected() => 
            EventSystem.current.currentSelectedGameObject == this.inputField.gameObject;

        private void LateUpdate()
        {
            if (this.CurrentInputSelected() && Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteEvents.Execute<IPointerClickHandler>(this.selectable.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                this.SelectCurrentInput();
            }
        }

        protected virtual void SelectCurrentInput()
        {
            this.inputField.Select();
        }

        protected virtual void Start()
        {
            if (this.overridenSelector != null)
            {
                this.overridenSelector.enabled = false;
            }
        }
    }
}

