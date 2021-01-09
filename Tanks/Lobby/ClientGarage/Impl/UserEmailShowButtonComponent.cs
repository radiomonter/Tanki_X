namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserEmailShowButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Color visibleColor;
        [SerializeField]
        private Color invisibleColor;

        public void SetEyeColor(bool visibly)
        {
            this.icon.color = !visibly ? this.visibleColor : this.invisibleColor;
        }
    }
}

