namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class SelectCountryFilter : MonoBehaviour
    {
        [SerializeField]
        private FilteredListDataProvider list;
        [SerializeField]
        private InputField inputField;

        private void ApplyFilter(string arg0)
        {
            this.list.ApplyFilter(new Func<object, bool>(this.IsFiltered));
        }

        private bool IsFiltered(object dataProvider)
        {
            KeyValuePair<string, string> pair = (KeyValuePair<string, string>) dataProvider;
            return (!string.IsNullOrEmpty(this.inputField.text) ? !pair.Value.StartsWith(this.inputField.text, StringComparison.CurrentCultureIgnoreCase) : false);
        }

        private void OnDisable()
        {
            this.inputField.onValueChanged.RemoveListener(new UnityAction<string>(this.ApplyFilter));
        }

        private void OnEnable()
        {
            this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.ApplyFilter));
        }
    }
}

