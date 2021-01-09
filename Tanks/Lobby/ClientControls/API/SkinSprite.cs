namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SkinSprite
    {
        [SerializeField]
        private string uid;
        [SerializeField]
        private UnityEngine.Sprite sprite;

        public string Uid =>
            this.uid;

        public UnityEngine.Sprite Sprite
        {
            get => 
                this.sprite;
            set => 
                this.sprite = value;
        }
    }
}

