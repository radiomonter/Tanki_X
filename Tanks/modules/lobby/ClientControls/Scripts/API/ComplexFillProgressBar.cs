namespace tanks.modules.lobby.ClientControls.Scripts.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ComplexFillProgressBar : MonoBehaviour
    {
        private const string INVALID_PROGRESS_FORMAT = "Incorrect ProgressValue {0}. The available ProgressValue's range is [0,1]";
        [SerializeField]
        private Image maskImage;
        private RectTransform maskImageRectTransform;
        private RectTransform parentRectTransform;
        private float val;

        public void Awake()
        {
            if (this.maskImage == null)
            {
                Mask componentInChildren = base.gameObject.GetComponentInChildren<Mask>();
                this.maskImage = componentInChildren.gameObject.GetComponent<Image>();
            }
            this.maskImageRectTransform = this.maskImage.GetComponent<RectTransform>();
            this.parentRectTransform = this.maskImage.transform.parent.GetComponent<RectTransform>();
        }

        public float ProgressValue
        {
            get => 
                this.val;
            set
            {
                if ((value < 0f) || (value > 1f))
                {
                    throw new ArgumentException($"Incorrect ProgressValue {value}. The available ProgressValue's range is [0,1]");
                }
                this.val = value;
                Vector2 offsetMax = this.maskImageRectTransform.offsetMax;
                offsetMax.x = (this.val - 1f) * this.parentRectTransform.rect.width;
                this.maskImageRectTransform.offsetMax = offsetMax;
            }
        }
    }
}

