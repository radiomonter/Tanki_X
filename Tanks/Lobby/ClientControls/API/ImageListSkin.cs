namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Image))]
    public class ImageListSkin : ImageSkin
    {
        [SerializeField]
        private List<string> uids = new List<string>();
        [SerializeField]
        private List<string> names = new List<string>();
        [SerializeField]
        private int selectedSpriteIndex;

        protected override void OnEnable()
        {
            if ((this.selectedSpriteIndex >= 0) && (this.selectedSpriteIndex < this.uids.Count))
            {
                base.SpriteUid = this.uids[this.selectedSpriteIndex];
            }
            base.OnEnable();
        }

        public void SelectSprite(string name)
        {
            int index = this.names.IndexOf(name);
            if (index < 0)
            {
                throw new SpriteNotFoundException(name);
            }
            base.SpriteUid = this.uids[index];
            this.selectedSpriteIndex = index;
        }

        public int SelectedSpriteIndex
        {
            get => 
                this.selectedSpriteIndex;
            set
            {
                this.selectedSpriteIndex = value;
                this.SelectSprite(this.names[this.selectedSpriteIndex]);
            }
        }

        public int Count =>
            this.uids.Count;

        public class SpriteNotFoundException : ArgumentException
        {
            public SpriteNotFoundException(string name) : base("Sprite with name " + name + " not found")
            {
            }
        }
    }
}

