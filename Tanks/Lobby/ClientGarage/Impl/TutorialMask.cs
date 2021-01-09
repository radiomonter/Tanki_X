namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class TutorialMask : MonoBehaviour
    {
        private RectTransform targetRect;
        private RectTransform thisRect;

        private void Awake()
        {
            this.thisRect = base.GetComponent<RectTransform>();
        }

        public void Init(RectTransform targetRect)
        {
            this.targetRect = targetRect;
        }

        private void Update()
        {
            this.thisRect.pivot = this.targetRect.pivot;
            this.thisRect.position = this.targetRect.position;
            this.thisRect.sizeDelta = new Vector2(this.targetRect.rect.width, this.targetRect.rect.height);
        }
    }
}

