namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public abstract class PostProcessingComponentBase
    {
        public PostProcessingContext context;

        protected PostProcessingComponentBase()
        {
        }

        public virtual DepthTextureMode GetCameraFlags() => 
            DepthTextureMode.None;

        public abstract PostProcessingModel GetModel();
        public virtual void OnDisable()
        {
        }

        public virtual void OnEnable()
        {
        }

        public abstract bool active { get; }
    }
}

