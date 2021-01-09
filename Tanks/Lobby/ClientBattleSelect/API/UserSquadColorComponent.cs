namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UserSquadColorComponent : BehaviourComponent
    {
        [SerializeField]
        private UnityEngine.UI.Image image;

        public UnityEngine.UI.Image Image
        {
            set => 
                this.image = value;
        }

        public UnityEngine.Color Color
        {
            set => 
                this.image.color = value;
        }
    }
}

