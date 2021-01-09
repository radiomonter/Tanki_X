namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class LineSlotView : MonoBehaviour
    {
        [SerializeField]
        private Image longLine;
        [SerializeField]
        private Image shortLine;

        private void OnDisable()
        {
            this.longLine.fillAmount = 0f;
            this.shortLine.fillAmount = 0f;
        }
    }
}

