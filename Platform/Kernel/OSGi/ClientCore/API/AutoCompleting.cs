namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;

    public class AutoCompleting : ActivatorCompletionStrategy
    {
        public void TryAutoCompletion(Action onComplete)
        {
            onComplete();
        }
    }
}

