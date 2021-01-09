namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeFieldResolverException : Exception
    {
        public NodeFieldResolverException(NodeClassInstanceDescription description, Exception e) : base("description = " + description, e)
        {
        }
    }
}

