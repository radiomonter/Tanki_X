namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeDescriptionException : Exception
    {
        public NodeDescriptionException(Type nodeClass) : base("Node = " + nodeClass)
        {
        }

        public NodeDescriptionException(Type nodeClass, string msg) : base($"Node = {nodeClass}, msg = {msg}")
        {
        }
    }
}

