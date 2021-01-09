namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface FlowListener
    {
        void OnFlowClean();
        void OnFlowFinish();
    }
}

