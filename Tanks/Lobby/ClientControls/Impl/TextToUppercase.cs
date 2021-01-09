namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class TextToUppercase : MonoBehaviour
    {
        private Text inputField;
        private TextMeshProUGUI tmpText;

        private void Start()
        {
            this.inputField = base.GetComponent<Text>();
            this.tmpText = base.GetComponent<TextMeshProUGUI>();
            this.ToUpperCase();
        }

        public void ToUpperCase()
        {
            if (this.inputField != null)
            {
                this.inputField.text = this.inputField.text.ToUpper();
            }
            else
            {
                this.tmpText.text = this.tmpText.text.ToUpper();
            }
        }
    }
}

