namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ConvertEntityToNodeException : Exception
    {
        public ConvertEntityToNodeException(NodeDescription nodeDescription, Entity entity) : base($"nodeDescription = {nodeDescription}, entity = {entity}")
        {
        }

        public ConvertEntityToNodeException(NodeDescription nodeDescription, Entity entity, Exception e) : base($"nodeDescription = {nodeDescription}, entity = {entity}", e)
        {
        }

        public ConvertEntityToNodeException(Type nodeClass, Entity entity, Exception e) : base($"nodeClass = {nodeClass}, entity = {entity}", e)
        {
        }
    }
}

