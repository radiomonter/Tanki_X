namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoginRewardProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image middleIcon;
        [SerializeField]
        private Image leftLine;
        [SerializeField]
        private Image rightLine;
        [SerializeField]
        private Color fillColor;
        [SerializeField]
        private Color emptyColor;

        public void Fill(FillType type)
        {
            this.middleIcon.color = ((type == FillType.Half) || (type == FillType.Full)) ? this.fillColor : this.emptyColor;
            this.leftLine.color = ((type == FillType.Half) || (type == FillType.Full)) ? this.fillColor : this.emptyColor;
            this.rightLine.color = (type != FillType.Full) ? this.emptyColor : this.fillColor;
        }

        public GameObject LeftLine =>
            this.leftLine.gameObject;

        public GameObject RightLine =>
            this.rightLine.gameObject;

        public enum FillType
        {
            Empty,
            Half,
            Full
        }
    }
}

