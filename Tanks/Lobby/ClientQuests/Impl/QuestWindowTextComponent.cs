namespace Tanks.Lobby.ClientQuests.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class QuestWindowTextComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI closeButton;

        public string CloseButton
        {
            set => 
                this.closeButton.text = value;
        }
    }
}

