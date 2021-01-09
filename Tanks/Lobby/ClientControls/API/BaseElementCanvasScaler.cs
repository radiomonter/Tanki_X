namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Canvas)), ExecuteInEditMode]
    public class BaseElementCanvasScaler : MonoBehaviour, BaseElementScaleControllerProvider
    {
        private bool resizing;
        private static bool initialized;
        [SerializeField]
        private Tanks.Lobby.ClientControls.API.BaseElementScaleController baseElementScaleController;

        protected void Awake()
        {
            if ((this.baseElementScaleController != null) && !initialized)
            {
                initialized = true;
                this.baseElementScaleController.Init();
            }
        }

        public static void MarkNeedInitialize()
        {
            initialized = false;
        }

        protected void OnEnable()
        {
        }

        protected void OnRectTransformDimensionsChange()
        {
            if (!this.resizing && ((base.enabled || !Application.isPlaying) && (this.baseElementScaleController != null)))
            {
                this.resizing = true;
                this.baseElementScaleController.Handle(base.GetComponent<Canvas>());
                this.resizing = false;
            }
        }

        public Tanks.Lobby.ClientControls.API.BaseElementScaleController BaseElementScaleController
        {
            get => 
                this.baseElementScaleController;
            set => 
                this.baseElementScaleController = value;
        }
    }
}

