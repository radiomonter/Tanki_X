namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class Cursors
    {
        private static Dictionary<CursorType, CursorData> type2Data = new Dictionary<CursorType, CursorData>();
        private static CursorData defaultCursorData;

        public static void Clean()
        {
            type2Data.Clear();
            defaultCursorData.texture = null;
            defaultCursorData.hotspot = Vector2.zero;
        }

        public static void InitCursor(CursorType type, Texture2D cursorTexture, Vector2 cursorHotspot)
        {
            if (cursorTexture == null)
            {
                LoggerProvider.GetLogger(typeof(Cursors)).ErrorFormat("CursorService:InitCursor argument 'cursorTexture' is null, argument 'type' is {0}", type);
            }
            else
            {
                type2Data.Add(type, new CursorData(cursorTexture, cursorHotspot));
            }
        }

        public static void InitDefaultCursor(Texture2D cursorTexture, Vector2 cursorHotspot)
        {
            if (cursorTexture != null)
            {
                defaultCursorData.hotspot = cursorHotspot;
                defaultCursorData.texture = cursorTexture;
            }
            else
            {
                LoggerProvider.GetLogger(typeof(Cursors)).Error("CursorService:InitDefaultCursor argument 'cursorTexture' is null");
                defaultCursorData.hotspot = Vector2.zero;
                defaultCursorData.texture = null;
            }
        }

        public static void SwitchToCursor(CursorType type)
        {
            CursorData data;
            if (type2Data.TryGetValue(type, out data))
            {
                Cursor.SetCursor(data.texture, data.hotspot, CursorMode.Auto);
            }
        }

        public static void SwitchToDefaultCursor()
        {
            Cursor.SetCursor(defaultCursorData.texture, defaultCursorData.hotspot, CursorMode.Auto);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CursorData
        {
            public Texture2D texture;
            public Vector2 hotspot;
            public CursorData(Texture2D texture, Vector2 hotspot)
            {
                this.texture = texture;
                this.hotspot = hotspot;
            }
        }
    }
}

