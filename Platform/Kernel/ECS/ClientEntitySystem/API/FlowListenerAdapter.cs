namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class FlowListenerAdapter : FlowListener
    {
        public static readonly FlowListenerAdapter Stub = new FlowListenerAdapter();

        public void OnFlowClean()
        {
        }

        public void OnFlowFinish()
        {
        }
    }
}

