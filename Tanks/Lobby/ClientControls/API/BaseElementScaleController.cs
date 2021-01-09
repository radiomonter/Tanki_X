namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BaseElementScaleController : ScriptableObject, SizeController
    {
        [SerializeField]
        private List<BaseElement> elements = new List<BaseElement>();
        private HashSet<SpriteRequest> requests = new HashSet<SpriteRequest>();
        private int resolutionIndex = -1;

        public void Handle(Canvas canvas)
        {
            if ((canvas != null) && (canvas.isRootCanvas && (this.elements.Count != 0)))
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    Debug.LogWarning("BaseElementCanvasScaler is not working in WorldSpace RenderMode");
                }
                int num = 0x7fffffff;
                int height = (int) canvas.pixelRect.height;
                int newResolutionIndex = -1;
                for (int i = 0; i < this.elements.Count; i++)
                {
                    int num5 = height - this.elements[i].CanvasHeight;
                    if ((num5 > 0) && (num5 < num))
                    {
                        num = num5;
                        newResolutionIndex = i;
                    }
                }
                if (newResolutionIndex >= 0)
                {
                    canvas.scaleFactor = ((float) this.elements[newResolutionIndex].Size) / 100f;
                    canvas.referencePixelsPerUnit = 100f / canvas.scaleFactor;
                }
                if (newResolutionIndex != this.resolutionIndex)
                {
                    this.ValidateSkin(this.resolutionIndex, newResolutionIndex);
                    if (Application.isPlaying && (this.resolutionIndex != newResolutionIndex))
                    {
                        canvas.BroadcastMessage("OnBaseElementSizeChanged", SendMessageOptions.DontRequireReceiver);
                    }
                    this.resolutionIndex = newResolutionIndex;
                }
            }
        }

        public void Init()
        {
            this.requests.Clear();
            foreach (BaseElement element in this.elements)
            {
                element.Init();
            }
        }

        public void OnDestroy()
        {
            foreach (BaseElement element in this.elements)
            {
                element.CancelAllRequests();
            }
            this.resolutionIndex = -1;
        }

        public void RegisterSpriteRequest(SpriteRequest request)
        {
            if (!this.requests.Contains(request))
            {
                this.requests.Add(request);
            }
            if ((this.resolutionIndex < 0) && (this.elements.Count > 0))
            {
                this.resolutionIndex = 0;
            }
            this.elements[this.resolutionIndex].RequestSprite(request);
        }

        public void UnregisterSpriteRequest(SpriteRequest request)
        {
            this.requests.Remove(request);
            if (this.resolutionIndex >= 0)
            {
                this.elements[this.resolutionIndex].CancelRequest(request);
            }
        }

        private void ValidateSkin(int oldResolutionIndex, int newResolutionIndex)
        {
            if (oldResolutionIndex >= 0)
            {
                this.elements[oldResolutionIndex].CancelAllRequests();
            }
            BaseElement element = this.elements[newResolutionIndex];
            foreach (SpriteRequest request in this.requests)
            {
                element.RequestSprite(request);
            }
        }

        public AssetReference LoadingSkin { get; set; }
    }
}

