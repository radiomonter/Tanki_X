namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine.EventSystems;

    public abstract class AbstractInputFieldSoundEffectController : UISoundEffectController, ISelectHandler, IDeselectHandler, IEventSystemHandler
    {
        private bool selected;

        protected AbstractInputFieldSoundEffectController()
        {
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this.selected = false;
        }

        private void OnEnable()
        {
            this.selected = false;
        }

        public void OnSelect(BaseEventData eventData)
        {
            this.selected = true;
        }

        protected bool Selected =>
            this.selected;
    }
}

