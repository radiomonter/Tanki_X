namespace Tanks.Lobby.ClientNotifications.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class CardCountComponent : MonoBehaviour
    {
        [SerializeField]
        private AnimatedLong count;
        [SerializeField]
        private NewItemNotificationUnityComponent card;

        private void Awake()
        {
            this.count.SetImmediate(-1L);
            this.count.Value = this.card.GetComponent<NewItemNotificationUnityComponent>().count;
        }
    }
}

