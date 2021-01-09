namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class FlowOverflowException : Exception
    {
        public FlowOverflowException(string stackTrace) : base(stackTrace)
        {
        }
    }
}

