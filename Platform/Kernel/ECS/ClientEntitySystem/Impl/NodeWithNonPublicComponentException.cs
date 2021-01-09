namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeWithNonPublicComponentException : NodeDescriptionException
    {
        public NodeWithNonPublicComponentException(Type nodeClass) : base(nodeClass)
        {
        }
    }
}

