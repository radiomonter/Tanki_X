namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class HandlerFactoryAlreadyRegisteredForFlowTaskException : Exception
    {
        public HandlerFactoryAlreadyRegisteredForFlowTaskException(Type taskType) : base(taskType.FullName)
        {
        }
    }
}

