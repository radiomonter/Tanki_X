namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Runtime.InteropServices;

    public interface Activator
    {
        void Launch(Action onComplete = null);
    }
}

