namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;

    public interface ActivatorCompletionStrategy
    {
        void TryAutoCompletion(Action onComplete);
    }
}

