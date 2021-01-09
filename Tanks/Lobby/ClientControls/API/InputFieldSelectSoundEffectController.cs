namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine.EventSystems;

    public class InputFieldSelectSoundEffectController : AbstractInputFieldSoundEffectController, IPointerDownHandler, IEventSystemHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!base.Selected)
            {
                base.PlaySoundEffect();
            }
        }

        public override string HandlerName =>
            "Select";
    }
}

