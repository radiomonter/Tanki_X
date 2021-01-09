namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ResizeDispatcher : MonoBehaviour
    {
        [SerializeField]
        private List<ResizeListener> listeners;

        private void OnEnable()
        {
            this.OnRectTransformDimensionsChange();
        }

        private void OnRectTransformDimensionsChange()
        {
            RectTransform component = base.GetComponent<RectTransform>();
            foreach (ResizeListener listener in this.listeners)
            {
                listener.OnResize(component);
            }
        }
    }
}

