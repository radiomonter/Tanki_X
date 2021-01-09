namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CursorSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        private static GameObject overObject;
        public CursorType cursorType;

        private void OnDisable()
        {
            if (overObject == base.gameObject)
            {
                overObject = null;
                Cursors.SwitchToDefaultCursor();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (((this.cursorType == CursorType.HAND) && base.gameObject.IsInteractable()) || (this.cursorType != CursorType.HAND))
            {
                Cursors.SwitchToCursor(this.cursorType);
                overObject = base.gameObject;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursors.SwitchToDefaultCursor();
        }
    }
}

