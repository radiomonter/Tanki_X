namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScrollText : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        public virtual string Text
        {
            get => 
                this.text.text;
            set => 
                this.text.text = value;
        }
    }
}

