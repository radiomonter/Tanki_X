namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ChatScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private EntityBehaviour chatDialogBehaviour;

        public void BuildDialog()
        {
            this.chatDialogBehaviour.gameObject.SetActive(true);
        }
    }
}

