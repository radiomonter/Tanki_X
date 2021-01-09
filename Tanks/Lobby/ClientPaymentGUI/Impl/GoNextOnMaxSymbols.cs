namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(TMP_InputField))]
    public class GoNextOnMaxSymbols : MonoBehaviour
    {
        private void Awake()
        {
            base.GetComponent<TMP_InputField>().onValueChanged.AddListener(new UnityAction<string>(this.ValueChanged));
        }

        private void ValueChanged(string val)
        {
            TMP_InputField component = base.GetComponent<TMP_InputField>();
            if (component.text.Length == component.characterLimit)
            {
                component.navigation.selectOnDown.Select();
            }
        }
    }
}

