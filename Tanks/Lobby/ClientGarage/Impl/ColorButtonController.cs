namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ColorButtonController : MonoBehaviour
    {
        [SerializeField]
        public List<ColorData> colors = new List<ColorData>();
        private ColorData currentColor = new ColorData();
        private ColorData lastColor;
        private List<IColorButtonElement> elements = new List<IColorButtonElement>();

        public void AddElement(IColorButtonElement element)
        {
            element.SetColor(this.currentColor);
            this.elements.Add(element);
        }

        public void Awake()
        {
            this.currentColor = this.GetDefaultColor();
        }

        private ColorData GetDefaultColor()
        {
            ColorData data = new ColorData();
            for (int i = 0; i < this.colors.Count; i++)
            {
                ColorData data2 = this.colors[i];
                if (data2.defaultColor)
                {
                    data = data2;
                }
            }
            return data;
        }

        public void RemoveElement(IColorButtonElement element)
        {
            this.elements.Remove(element);
        }

        public void SetColor(int i)
        {
            this.currentColor = this.colors[i];
            this.SetColor(this.currentColor);
        }

        private void SetColor(ColorData color)
        {
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements[i].SetColor(color);
            }
        }

        public void SetDefault()
        {
            ColorData defaultColor = this.GetDefaultColor();
            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements[i].SetColor(defaultColor);
            }
            this.currentColor = defaultColor;
        }
    }
}

