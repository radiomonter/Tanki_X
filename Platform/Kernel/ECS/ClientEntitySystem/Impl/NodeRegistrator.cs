namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeRegistrator
    {
        public void Register(Type nodeType, ICollection<Type> additionalComponents)
        {
            NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(nodeType, additionalComponents));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

