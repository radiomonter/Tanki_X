namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class ScreenScaller : MonoBehaviour
    {
        [SerializeField]
        private int screenHeight;
        [SerializeField]
        private int aspectRatioX;
        [SerializeField]
        private int aspectRatioY;

        private void Update()
        {
            if (base.GetComponentInParent<Canvas>() != null)
            {
                float height = base.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;
                base.GetComponent<RectTransform>().localScale = ((((float) this.aspectRatioX) / ((float) this.aspectRatioY)) <= (((float) Screen.width) / ((float) Screen.height))) ? ((Vector3.one * height) / ((float) this.screenHeight)) : ((Vector3.one * this.screenHeight) / height);
            }
        }
    }
}

