namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class NotificationInstanceComponent : Component
    {
        public NotificationInstanceComponent(GameObject instance)
        {
            this.Instance = instance;
        }

        public GameObject Instance { get; set; }
    }
}

