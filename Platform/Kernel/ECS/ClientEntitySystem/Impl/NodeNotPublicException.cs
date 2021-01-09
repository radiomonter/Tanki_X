namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeNotPublicException : NodeDescriptionException
    {
        public NodeNotPublicException(Type nodeClass) : base(nodeClass)
        {
        }
    }
}

