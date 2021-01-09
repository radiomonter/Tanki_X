namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MakeKeyboardSettingsButton : MonoBehaviour
    {
        [ContextMenu("Make")]
        public void Make()
        {
            Debug.Log("Make for " + base.gameObject.name);
            MakeAllNotIneractable ineractable = base.gameObject.AddComponent<MakeAllNotIneractable>();
            ineractable.MakeNotInteractable();
            DestroyImmediate(ineractable);
            Debug.Log("Not interactable");
            foreach (InputField field in base.GetComponentsInChildren<InputField>())
            {
                Debug.Log(field.name);
                GameObject obj2 = new GameObject("Button");
                obj2.transform.SetParent(base.transform, false);
                obj2.AddComponent<Button>();
                obj2.AddComponent<CursorSwitcher>();
                obj2.AddComponent<InputFieldParentButton>();
                Image image = obj2.AddComponent<Image>();
                image.color = Color.clear;
                RectTransform component = field.GetComponent<RectTransform>();
                RectTransform transform2 = obj2.GetComponent<RectTransform>();
                transform2.pivot = component.pivot;
                transform2.anchorMax = component.anchorMax;
                transform2.anchorMin = component.anchorMin;
                transform2.anchoredPosition = component.anchoredPosition;
                transform2.offsetMin = component.offsetMin;
                transform2.offsetMax = component.offsetMax;
                field.transform.SetParent(obj2.transform, false);
                component.anchorMin = Vector2.zero;
                component.anchorMax = Vector2.one;
                Vector2 zero = Vector2.zero;
                component.offsetMax = zero;
                component.offsetMin = zero;
            }
            Debug.Log("Done");
        }
    }
}

