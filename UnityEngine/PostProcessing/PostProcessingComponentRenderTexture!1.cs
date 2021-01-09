namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T: PostProcessingModel
    {
        protected PostProcessingComponentRenderTexture()
        {
        }

        public virtual void Prepare(Material material)
        {
        }
    }
}

