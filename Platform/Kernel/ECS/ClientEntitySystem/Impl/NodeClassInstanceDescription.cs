namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class NodeClassInstanceDescription
    {
        public static readonly NodeClassInstanceDescription EMPTY = new NodeClassInstanceDescription(typeof(Node), AbstractNodeDescription.EMPTY);
        private readonly Type nodeClass;
        private Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescription nodeDescription;
        private readonly IDictionary<Type, FieldInfo> fieldByComponent;

        public NodeClassInstanceDescription(Type nodeClass, Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescription nodeDescription)
        {
            this.nodeClass = nodeClass;
            this.nodeDescription = nodeDescription;
            this.fieldByComponent = this.CreateAndPopulateFieldByComponent();
        }

        private IDictionary<Type, FieldInfo> CreateAndPopulateFieldByComponent()
        {
            IDictionary<Type, FieldInfo> dictionary = new Dictionary<Type, FieldInfo>();
            foreach (FieldInfo info in this.nodeClass.GetFields())
            {
                if (typeof(Component).IsAssignableFrom(info.FieldType))
                {
                    dictionary[info.FieldType] = info;
                }
            }
            return dictionary;
        }

        public Node CreateNode(EntityInternal entity)
        {
            Node node2;
            Node nodeInstance = Cache.GetNodeInstance(this.nodeClass);
            nodeInstance.Entity = entity;
            try
            {
                Collections.Enumerator<Type> enumerator = Collections.GetEnumerator<Type>(this.nodeDescription.Components);
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        node2 = nodeInstance;
                        break;
                    }
                    Type current = enumerator.Current;
                    this.SetComponent(nodeInstance, current, entity.GetComponent(current));
                }
            }
            catch (Exception exception)
            {
                throw new ConvertEntityToNodeException(this.nodeClass, entity, exception);
            }
            return node2;
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(this, o))
            {
                return true;
            }
            if ((o == null) || !ReferenceEquals(base.GetType(), o.GetType()))
            {
                return false;
            }
            NodeClassInstanceDescription description = (NodeClassInstanceDescription) o;
            return ReferenceEquals(this.NodeClass, description.NodeClass);
        }

        public void FreeNode(Node node)
        {
            Cache.FreeNodeInstance(node);
        }

        public override int GetHashCode() => 
            0x275 + this.NodeClass.GetHashCode();

        public void SetComponent(Node node, Type componentClass, Component component)
        {
            if (this.fieldByComponent.ContainsKey(componentClass))
            {
                this.fieldByComponent[componentClass].SetValue(node, component);
            }
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescription NodeDescription
        {
            get => 
                this.nodeDescription;
            set => 
                this.nodeDescription = value;
        }

        public Type NodeClass =>
            this.nodeClass;
    }
}

