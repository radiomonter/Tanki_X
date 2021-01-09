namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeInheritanceException : NodeDescriptionException
    {
        public NodeInheritanceException(Type nodeClass) : base(nodeClass)
        {
        }
    }
}

