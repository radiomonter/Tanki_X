namespace Platform.Library.ClientUnityIntegration
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.InteropServices;

    public abstract class UnityAwareActivator<TCompletionStrategy> : ECSBehaviour, Activator where TCompletionStrategy: ActivatorCompletionStrategy, new()
    {
        private Action onComplete;

        protected UnityAwareActivator()
        {
        }

        protected virtual void Activate()
        {
        }

        protected void Complete()
        {
            LoggerProvider.GetLogger(this).InfoFormat("Complete {0}", this);
            if (this.onComplete != null)
            {
                this.onComplete();
            }
        }

        public void Launch(Action onComplete = null)
        {
            LoggerProvider.GetLogger(this).InfoFormat("Activate {0}", this);
            this.onComplete = onComplete;
            this.Activate();
            Activator.CreateInstance<TCompletionStrategy>().TryAutoCompletion(new Action(this.Complete));
        }

        public void OnEnable()
        {
        }
    }
}

