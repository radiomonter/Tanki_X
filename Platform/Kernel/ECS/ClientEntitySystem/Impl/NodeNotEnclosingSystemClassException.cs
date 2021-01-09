namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeNotEnclosingSystemClassException : NodeDescriptionException
    {
        public NodeNotEnclosingSystemClassException(Type nodeClass) : base(nodeClass)
        {
        }
    }
}

