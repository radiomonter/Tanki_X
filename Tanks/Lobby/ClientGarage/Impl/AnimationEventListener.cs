namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class AnimationEventListener : MonoBehaviour
    {
        private Action hideHandler;
        private Action partyHandler;

        public void OnHide()
        {
            if (this.hideHandler != null)
            {
                this.hideHandler();
            }
        }

        public void OnPartyFinish()
        {
            if (this.partyHandler != null)
            {
                this.partyHandler();
                this.partyHandler = null;
            }
        }

        public void SetHideHandler(Action handler)
        {
            this.hideHandler = handler;
        }

        public void SetPartyHandler(Action handler)
        {
            this.partyHandler = handler;
        }
    }
}

