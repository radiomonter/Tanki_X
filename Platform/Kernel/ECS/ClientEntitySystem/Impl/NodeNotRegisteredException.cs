namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NodeNotRegisteredException : Exception
    {
        public NodeNotRegisteredException(NodeDescription nodeDescription) : base("Node not registered: " + nodeDescription + "\npublic void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager, EngineServiceInternal engineService, NodeRegistrator nodeRegistrator) {\n\tbase(...)\n\tnodeRegistrator.Register(typeof(MyNode));\n}\n")
        {
        }
    }
}

