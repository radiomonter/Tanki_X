namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Text;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    [RequireComponent(typeof(TMP_InputField))]
    public abstract class BaseTMPInputFormatter : MonoBehaviour
    {
        protected TMP_InputField input;
        private bool formating;

        protected BaseTMPInputFormatter()
        {
        }

        private void Awake()
        {
            this.input = base.GetComponent<TMP_InputField>();
            this.input.onValueChanged.AddListener(new UnityAction<string>(this.Format));
        }

        protected abstract string ClearFormat(string text);
        private void Format(string text)
        {
            if (!this.formating)
            {
                this.formating = true;
                StringBuilder builder = new StringBuilder();
                text = this.ClearFormat(text);
                int num = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    string str = this.FormatAt(text[i], i);
                    builder.Append(str);
                    num += str.Length - 1;
                }
                this.input.text = builder.ToString();
                this.input.stringPosition += num;
                this.input.text = builder.ToString();
                this.formating = false;
            }
        }

        protected abstract string FormatAt(char symbol, int charIndex);
    }
}

