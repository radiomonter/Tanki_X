namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class LayoutRebuildMarker : UIBehaviour
    {
        public RectTransform target;

        public void Init(RectTransform target)
        {
            this.target = target;
        }

        protected override void OnRectTransformDimensionsChange()
        {
            LayoutRebuilder.MarkLayoutForRebuild(this.target);
        }
    }
}

