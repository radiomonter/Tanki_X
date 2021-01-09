namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine.Rendering;

    public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T: PostProcessingModel
    {
        protected PostProcessingComponentCommandBuffer()
        {
        }

        public abstract CameraEvent GetCameraEvent();
        public abstract string GetName();
        public abstract void PopulateCommandBuffer(CommandBuffer cb);
    }
}

