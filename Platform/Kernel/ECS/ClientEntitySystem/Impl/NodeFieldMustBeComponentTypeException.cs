namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeFieldMustBeComponentTypeException : ECSNotRunningException
    {
        public NodeFieldMustBeComponentTypeException(string str) : base(str)
        {
        }
    }
}

