namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class RandomImageSelector : MonoBehaviour
    {
        public Image TargetImage;
        public Sprite[] AvailableSprites;

        public void ChangeImage()
        {
            if (this.AvailableSprites.Length > 0)
            {
                int index = Random.Range(0, this.AvailableSprites.Length);
                this.TargetImage.sprite = this.AvailableSprites[index];
            }
        }

        private void OnEnable()
        {
            this.ChangeImage();
        }
    }
}

