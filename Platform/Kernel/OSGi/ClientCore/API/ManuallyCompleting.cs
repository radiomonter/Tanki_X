namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;

    public class ManuallyCompleting : ActivatorCompletionStrategy
    {
        public void TryAutoCompletion(Action onComplete)
        {
        }
    }
}

