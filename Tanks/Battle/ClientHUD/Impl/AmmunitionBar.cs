namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class AmmunitionBar : MonoBehaviour
    {
        [SerializeField]
        private Image fillImage;
        [SerializeField]
        private Image light;

        public void Activate()
        {
            this.light.gameObject.SetActive(true);
            this.FillValue = 1f;
        }

        public void Deactivate()
        {
            this.light.gameObject.SetActive(false);
            this.FillValue = 0f;
        }

        public float FillValue
        {
            set => 
                this.fillImage.rectTransform.anchorMax = new Vector2(value, 1f);
        }
    }
}

