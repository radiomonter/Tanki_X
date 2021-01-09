namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine.EventSystems;

    public class UIElementSubmitSoundEffectController : UISoundEffectController, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
    {
        private const string HANDLER_NAME = "ClickAndSubmit";

        public void OnPointerClick(PointerEventData eventData)
        {
            if (base.gameObject.IsInteractable())
            {
                base.PlaySoundEffect();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            base.PlaySoundEffect();
        }

        public override string HandlerName =>
            "ClickAndSubmit";
    }
}

