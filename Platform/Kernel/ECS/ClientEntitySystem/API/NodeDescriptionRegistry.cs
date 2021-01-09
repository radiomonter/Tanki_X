namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface NodeDescriptionRegistry
    {
        void AddNodeDescription(NodeDescription nodeDescription);
        void AssertRegister(NodeDescription nodeDescription);
        ICollection<NodeClassInstanceDescription> GetClassInstanceDescriptionByComponent(Type component);
        ICollection<NodeDescription> GetNodeDescriptions(Type componentClass);
        ICollection<NodeDescription> GetNodeDescriptionsByNotComponent(Type componentClass);
        ICollection<NodeDescription> GetNodeDescriptionsWithNotComponentsOnly();
        NodeClassInstanceDescription GetOrCreateNodeClassDescription(Type nodeClass, ICollection<Type> additionalComponents = null);
    }
}

