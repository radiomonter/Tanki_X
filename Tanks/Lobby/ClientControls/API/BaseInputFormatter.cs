namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public abstract class BaseInputFormatter : MonoBehaviour
    {
        protected InputField input;
        private bool formating;

        protected BaseInputFormatter()
        {
        }

        private void Awake()
        {
            this.input = base.GetComponent<InputField>();
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
                this.input.caretPosition += num;
                this.formating = false;
            }
        }

        protected abstract string FormatAt(char symbol, int charIndex);
    }
}

