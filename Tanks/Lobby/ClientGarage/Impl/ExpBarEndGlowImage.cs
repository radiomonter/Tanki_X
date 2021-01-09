namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ExpBarEndGlowImage : MonoBehaviour
    {
        [SerializeField]
        private float minX;
        [SerializeField]
        private float maxX;
        [SerializeField]
        private UIRectClipper clipper;
        [SerializeField]
        private GameObject icon;
        private RectTransform thisRect;
        private RectTransform parentRect;

        private void Awake()
        {
            this.thisRect = base.GetComponent<RectTransform>();
            this.parentRect = base.transform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            float x = this.parentRect.rect.width * this.clipper.ToX;
            bool flag = (x < this.maxX) && (x > this.minX);
            this.icon.SetActive(flag);
            this.thisRect.anchoredPosition = new Vector2(x, 0f);
        }
    }
}

