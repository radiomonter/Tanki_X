namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeStars : MonoBehaviour
    {
        [SerializeField]
        private Image[] stars;

        public void SetPower(float power)
        {
            if (power < 0f)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                foreach (Image image in this.stars)
                {
                    float a = 1f / ((float) this.stars.Length);
                    float num3 = Mathf.Min(a, power);
                    power -= num3;
                    image.fillAmount = num3 / a;
                }
            }
        }
    }
}

