namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Text))]
    public class TopTextDescriptionItem : MonoBehaviour, ILayoutSelfController, ILayoutController
    {
        [SerializeField]
        public Scrollbar scroll;

        public void SetLayoutHorizontal()
        {
        }

        public void SetLayoutVertical()
        {
            float num;
            float num2;
            float num3;
            if (this.scroll.gameObject.activeSelf)
            {
                num = 1f;
                num2 = 1f;
                num3 = 1f;
            }
            else
            {
                num = 0f;
                num2 = 0f;
                num3 = 0f;
            }
            RectTransform component = base.GetComponent<RectTransform>();
            Vector2 anchorMin = component.anchorMin;
            Vector2 anchorMax = component.anchorMax;
            anchorMin.y = num;
            anchorMax.y = num2;
            component.anchorMin = anchorMin;
            component.anchorMax = anchorMax;
            Vector2 pivot = component.pivot;
            pivot.y = num3;
            component.pivot = pivot;
        }

        public string text
        {
            get => 
                base.GetComponent<Text>().text;
            set
            {
                base.GetComponent<Text>().text = value;
                if (this.scroll != null)
                {
                    this.scroll.value = 1f;
                }
                LayoutRebuilder.MarkLayoutForRebuild(base.GetComponent<RectTransform>());
            }
        }
    }
}

