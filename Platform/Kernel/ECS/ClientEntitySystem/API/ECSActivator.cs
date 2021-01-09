namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;

    public interface ECSActivator : Activator
    {
        void RegisterSystemsAndTemplates();
    }
}

