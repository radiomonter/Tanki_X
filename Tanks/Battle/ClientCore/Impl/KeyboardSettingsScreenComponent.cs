namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [SerialVersionUID(0x158716d7687L)]
    public class KeyboardSettingsScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject defaultButton;
        [SerializeField]
        private GameObject oneKeyOnFewActionsTextLabel;

        public void CheckForOneKeyOnFewActions()
        {
            KeyboardSettingsInputComponent[] componentsInChildren = base.GetComponentsInChildren<KeyboardSettingsInputComponent>(true);
            List<KeyCode> list = new List<KeyCode>();
            List<KeyCode> list2 = new List<KeyCode>();
            foreach (KeyboardSettingsInputComponent component in componentsInChildren)
            {
                component.SetInputState(false);
                KeyCode item = component.LoadAction();
                if (item != KeyCode.None)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                    else if (!list2.Contains(item))
                    {
                        list2.Add(item);
                    }
                }
            }
            foreach (KeyboardSettingsInputComponent component2 in componentsInChildren)
            {
                KeyCode item = component2.LoadAction();
                if (list2.Contains(item))
                {
                    component2.SetInputState(true);
                }
            }
            this.oneKeyOnFewActionsTextLabel.gameObject.SetActive(list2.Count > 0);
        }

        public void EnableButtons()
        {
        }

        private void OnEnable()
        {
            this.CheckForOneKeyOnFewActions();
        }
    }
}

