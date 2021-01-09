namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    public class DeserterDescriptionUIComponent : MonoBehaviour
    {
        public void Hide()
        {
            base.gameObject.SetActive(false);
        }

        public void ShowDescription(string text)
        {
            this.Rect.sizeDelta = new Vector2(this.Rect.sizeDelta.x, 50f);
            TextMeshProUGUI componentInChildren = base.GetComponentInChildren<TextMeshProUGUI>(true);
            componentInChildren.text = text;
            componentInChildren.gameObject.SetActive(true);
            base.gameObject.SetActive(true);
        }

        public RectTransform Rect =>
            base.GetComponent<RectTransform>();
    }
}

