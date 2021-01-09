namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputFieldReturnSelector : InputFieldReturnSelector
    {
        private TMP_InputField tmpInputField;

        protected override bool CurrentInputSelected() => 
            EventSystem.current.currentSelectedGameObject == this.tmpInputField.gameObject;

        protected override void SelectCurrentInput()
        {
            if (!(base.selectable is TMP_InputField))
            {
                this.tmpInputField.Select();
            }
        }

        protected override void Start()
        {
            base.Start();
            this.tmpInputField = base.GetComponent<TMP_InputField>();
        }
    }
}

