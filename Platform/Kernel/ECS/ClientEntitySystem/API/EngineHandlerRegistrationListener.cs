namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;

    public interface EngineHandlerRegistrationListener
    {
        void OnHandlerAdded(Handler handler);
    }
}

