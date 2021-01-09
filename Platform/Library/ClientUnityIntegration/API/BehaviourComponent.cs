namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public abstract class BehaviourComponent : ECSBehaviour, Component
    {
        protected BehaviourComponent()
        {
        }
    }
}

