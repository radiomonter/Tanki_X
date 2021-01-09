namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class TutorialArrow : MonoBehaviour
    {
        private RectTransform arrowPositionRect;
        private RectTransform thisRect;

        private void Awake()
        {
            this.thisRect = base.GetComponent<RectTransform>();
        }

        public void Setup(RectTransform arrowPositionRect)
        {
            this.arrowPositionRect = arrowPositionRect;
        }

        private void Update()
        {
            this.thisRect.pivot = this.arrowPositionRect.pivot;
            this.thisRect.sizeDelta = this.arrowPositionRect.sizeDelta;
            this.thisRect.position = this.arrowPositionRect.position;
            this.thisRect.rotation = this.arrowPositionRect.rotation;
        }
    }
}

