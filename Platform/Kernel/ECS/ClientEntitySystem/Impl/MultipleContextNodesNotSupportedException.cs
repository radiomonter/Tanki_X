namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class MultipleContextNodesNotSupportedException : Exception
    {
        public MultipleContextNodesNotSupportedException(MethodInfo methodInfo) : base("Method: " + methodInfo)
        {
        }
    }
}

