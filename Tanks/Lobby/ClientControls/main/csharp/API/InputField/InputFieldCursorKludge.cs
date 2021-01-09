namespace Tanks.Lobby.ClientControls.main.csharp.API.InputField
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class InputFieldCursorKludge : MonoBehaviour
    {
        public InputFieldComponent inputField;
        public float yPivotOffset;

        private unsafe void Start()
        {
            RectTransform component = this.inputField.InputField.gameObject.GetComponent<RectTransform>();
            Vector2 pivot = component.pivot;
            Vector2* vectorPtr1 = &pivot;
            vectorPtr1->y += this.yPivotOffset;
            component.pivot = pivot;
        }
    }
}

