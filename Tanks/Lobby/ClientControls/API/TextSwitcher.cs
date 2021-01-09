namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TextSwitcher : MonoBehaviour
    {
        public Text nextText;
        public Text currentText;

        public void Switch()
        {
            string text = this.nextText.text;
            this.nextText.text = this.currentText.text;
            this.currentText.text = text;
        }
    }
}

