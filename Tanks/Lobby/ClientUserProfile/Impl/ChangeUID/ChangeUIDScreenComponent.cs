namespace Tanks.Lobby.ClientUserProfile.Impl.ChangeUID
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeUIDScreenComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private Text inputHint;

        public string InputHint
        {
            set => 
                this.inputHint.text = value;
        }
    }
}

