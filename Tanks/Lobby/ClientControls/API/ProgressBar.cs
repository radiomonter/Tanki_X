namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ProgressBar : MonoBehaviour
    {
        private const string INVALID_PROGRESS_FORMAT = "Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]";
        [SerializeField]
        private Image maskImage;

        public void Awake()
        {
            if (this.maskImage == null)
            {
                Mask componentInChildren = base.gameObject.GetComponentInChildren<Mask>();
                this.maskImage = componentInChildren.gameObject.GetComponent<Image>();
            }
        }

        public float ProgressValue
        {
            get => 
                this.maskImage.fillAmount;
            set
            {
                if ((value < 0f) || (value > 1f))
                {
                    throw new ArgumentException($"Incorrect ProgressValue {value}. The available ProgressValue's range is [0,1]");
                }
                this.maskImage.fillAmount = value;
            }
        }
    }
}

