namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ChatMessageInputComponent : MonoBehaviour, Component
    {
        private void Start()
        {
            base.gameObject.SetActive(false);
            base.gameObject.SetActive(true);
        }
    }
}

