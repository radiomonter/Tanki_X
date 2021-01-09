namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class CursorsConfigurator : MonoBehaviour
    {
        public Texture2D regularCursor;
        public Vector2 regularCursorHotspot;
        public Texture2D handCursor;
        public Vector2 handCursorHotspot;
        public Texture2D inputCursor;
        public Vector2 inputCursorHotspot;

        private void Awake()
        {
            Cursors.InitDefaultCursor(this.regularCursor, this.regularCursorHotspot);
            Cursors.InitCursor(CursorType.HAND, this.handCursor, this.handCursorHotspot);
            Cursors.InitCursor(CursorType.INPUT, this.inputCursor, this.inputCursorHotspot);
            Cursors.SwitchToDefaultCursor();
        }
    }
}

