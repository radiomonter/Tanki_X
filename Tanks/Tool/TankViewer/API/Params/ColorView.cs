namespace Tanks.Tool.TankViewer.API.Params
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ColorView : MonoBehaviour
    {
        public InputField colorInput;

        private void Awake()
        {
            this.colorInput.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
        }

        public Color GetColor()
        {
            Color color;
            return (!ColorUtility.TryParseHtmlString("#" + this.colorInput.text, out color) ? Color.white : color);
        }

        public void OnEndEdit(string inputText)
        {
            Color color;
            if (!ColorUtility.TryParseHtmlString("#" + this.colorInput.text, out color))
            {
                this.colorInput.text = "ffffff";
            }
        }

        public void SetColor(Color color)
        {
            this.colorInput.text = ColorUtility.ToHtmlStringRGB(color).ToLower();
        }
    }
}

