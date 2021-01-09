namespace UnityEngine.PostProcessing
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T: PostProcessingModel
    {
        protected PostProcessingComponent()
        {
        }

        public override PostProcessingModel GetModel() => 
            this.model;

        public virtual void Init(PostProcessingContext pcontext, T pmodel)
        {
            base.context = pcontext;
            this.model = pmodel;
        }

        public T model { get; internal set; }
    }
}

