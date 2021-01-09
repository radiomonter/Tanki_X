namespace CurvedUI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CurvedUIPointerEventData : PointerEventData
    {
        public GameObject Controller;
        public Vector2 TouchPadAxis;

        public CurvedUIPointerEventData(EventSystem eventSystem) : base(eventSystem)
        {
            this.TouchPadAxis = Vector2.zero;
        }

        public enum ControllerType
        {
            NONE = -1,
            VIVE = 0
        }
    }
}

