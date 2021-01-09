namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class InputFieldParentButton : MonoBehaviour
    {
        private void Start()
        {
            InputField componentInChildren = base.GetComponentInChildren<InputField>();
            Button component = base.GetComponent<Button>();
            if ((componentInChildren != null) && (component != null))
            {
                base.GetComponent<Button>().onClick.AddListener(new UnityAction(componentInChildren.Select));
            }
        }
    }
}

