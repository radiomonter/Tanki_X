namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class IncomingFriendsCounterComponent : BehaviourComponent
    {
        private int count;

        public int Count
        {
            get => 
                this.count;
            set
            {
                this.count = value;
                base.GetComponent<TextMeshProUGUI>().text = (this.count <= 0) ? string.Empty : ("[" + this.count + "]");
            }
        }
    }
}

