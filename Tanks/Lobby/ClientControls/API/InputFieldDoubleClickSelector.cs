namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class InputFieldDoubleClickSelector : MonoBehaviour
    {
        private static readonly float DOUBLE_CLICK_DELTA_MIN = 0.05f;
        private static readonly float DOUBLE_CLICK_DELTA_MAX = 0.3f;
        public InputFieldComponent inputField;
        private float timeBetweenClicks;
        private bool firstClick;
        private bool selected;
        private float selectionTime;

        public void OnMouseClick()
        {
            if (this.selectionTime >= DOUBLE_CLICK_DELTA_MAX)
            {
                this.firstClick = !this.firstClick;
                if ((this.timeBetweenClicks >= DOUBLE_CLICK_DELTA_MIN) && (this.timeBetweenClicks <= DOUBLE_CLICK_DELTA_MAX))
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(this.inputField.InputFieldGameObject);
                    this.firstClick = false;
                    this.timeBetweenClicks = 0f;
                }
            }
        }

        private void Update()
        {
            this.selected = EventSystem.current.currentSelectedGameObject == this.inputField.InputFieldGameObject;
            this.selectionTime = !this.selected ? 0f : (this.selectionTime + Time.deltaTime);
            if (this.firstClick)
            {
                this.timeBetweenClicks += Time.deltaTime;
            }
            if (this.timeBetweenClicks >= DOUBLE_CLICK_DELTA_MAX)
            {
                this.timeBetweenClicks = 0f;
                this.firstClick = false;
            }
        }
    }
}

