namespace Tanks.Lobby.ClientQuests.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class QuestItemGUITextComponent : LocalizedControl
    {
        [SerializeField]
        private TextMeshProUGUI progress;
        [SerializeField]
        private TextMeshProUGUI pickUp;
        [SerializeField]
        private TextMeshProUGUI nextQuest;

        public string Progress
        {
            set => 
                this.progress.text = value;
        }

        public string PickUp
        {
            set => 
                this.pickUp.text = value;
        }

        public string NextQuest
        {
            set => 
                this.nextQuest.text = value;
        }
    }
}

