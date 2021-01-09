namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class DefaultActivator<TCompletionStrategy> : Activator where TCompletionStrategy: ActivatorCompletionStrategy, new()
    {
        private Action onComplete;

        protected DefaultActivator()
        {
        }

        protected abstract void Activate();
        protected void Complete()
        {
            if (this.onComplete != null)
            {
                this.onComplete();
            }
        }

        public void Launch(Action onComplete = null)
        {
            this.onComplete = onComplete;
            this.Activate();
            Activator.CreateInstance<TCompletionStrategy>().TryAutoCompletion(new Action(this.Complete));
        }
    }
}

