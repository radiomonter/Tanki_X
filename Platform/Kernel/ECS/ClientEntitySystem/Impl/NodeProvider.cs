namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeProvider
    {
        private readonly EntityInternal entity;
        private readonly Node emptyNode;
        private readonly Dictionary<NodeClassInstanceDescription, Node> nodeByDescription = new Dictionary<NodeClassInstanceDescription, Node>(10);
        private readonly Dictionary<Node, NodeClassInstanceDescription> descriptionByNode = new Dictionary<Node, NodeClassInstanceDescription>(10);

        public NodeProvider(EntityInternal entity)
        {
            this.entity = entity;
            this.emptyNode = new Node();
            this.emptyNode.Entity = entity;
        }

        private void AssertCanCast(NodeDescription nodeDescription)
        {
            if (!this.CanCast(nodeDescription))
            {
                throw new ConvertEntityToNodeException(nodeDescription, this.entity);
            }
        }

        public bool CanCast(NodeDescription nodeDescription) => 
            !nodeDescription.IsEmpty ? this.entity.Contains(nodeDescription) : true;

        public void CleanNodes()
        {
            Dictionary<NodeClassInstanceDescription, Node>.Enumerator enumerator = this.nodeByDescription.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<NodeClassInstanceDescription, Node> current = enumerator.Current;
                NodeClassInstanceDescription key = current.Key;
                Node node = current.Value;
                key.FreeNode(node);
            }
            this.nodeByDescription.Clear();
            this.descriptionByNode.Clear();
        }

        private Node CreateNode(NodeClassInstanceDescription description)
        {
            Node node = description.CreateNode(this.entity);
            this.descriptionByNode[node] = description;
            return node;
        }

        public Node GetNode(NodeClassInstanceDescription nodeClassInstanceDescription)
        {
            Node node;
            if (ReferenceEquals(nodeClassInstanceDescription, NodeClassInstanceDescription.EMPTY))
            {
                return this.emptyNode;
            }
            this.AssertCanCast(nodeClassInstanceDescription.NodeDescription);
            if (!this.nodeByDescription.TryGetValue(nodeClassInstanceDescription, out node))
            {
                node = this.CreateNode(nodeClassInstanceDescription);
                this.nodeByDescription[nodeClassInstanceDescription] = node;
            }
            return node;
        }

        public void OnComponentAdded(Component component)
        {
            this.UpdateComponentValue(component, component.GetType());
        }

        public void OnComponentAdded(Component component, Type componentType)
        {
            this.UpdateComponentValue(component, componentType);
        }

        public void OnComponentChanged(Component component)
        {
            this.UpdateComponentValue(component, component.GetType());
        }

        private void UpdateComponentValue(Component component, Type componentType)
        {
            IEnumerator<NodeClassInstanceDescription> enumerator = NodeDescriptionRegistry.GetClassInstanceDescriptionByComponent(componentType).GetEnumerator();
            while (enumerator.MoveNext())
            {
                Node node;
                NodeClassInstanceDescription current = enumerator.Current;
                if (this.nodeByDescription.TryGetValue(current, out node))
                {
                    current.SetComponent(node, componentType, component);
                }
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

