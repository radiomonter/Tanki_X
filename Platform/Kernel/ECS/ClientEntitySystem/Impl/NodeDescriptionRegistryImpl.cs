namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class NodeDescriptionRegistryImpl : NodeDescriptionRegistry
    {
        private readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByAnyComponent = new Dictionary<Type, ICollection<NodeDescription>>();
        private readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionsByNotComponent = new Dictionary<Type, ICollection<NodeDescription>>();
        private readonly ICollection<NodeDescription> nodeDescriptionsWithNotComponentsOnly = new HashSet<NodeDescription>();
        private readonly ICollection<NodeDescription> nodeDescriptions = new HashSet<NodeDescription>();
        private readonly IDictionary<Type, NodeClassInstanceDescription> nodeClassDescByNodeClass = new Dictionary<Type, NodeClassInstanceDescription>();
        private readonly MultiMap<Type, NodeClassInstanceDescription> nodeClassDescsByComponent = new MultiMap<Type, NodeClassInstanceDescription>();
        private readonly Dictionary<NodeDescription, NodeDescription> nodeDescriptionStorage = new Dictionary<NodeDescription, NodeDescription>();
        [CompilerGenerated]
        private static Func<NodeDescription, NodeDescription> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ICollection<NodeDescription>> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<ICollection<NodeDescription>> <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<NodeDescription, NodeDescription> <>f__am$cache1;

        public void AddNodeDescription(NodeDescription nodeDescription)
        {
            <AddNodeDescription>c__AnonStorey0 storey = new <AddNodeDescription>c__AnonStorey0 {
                nodeDescription = nodeDescription,
                $this = this
            };
            if (!storey.nodeDescription.IsEmpty)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = d => d;
                }
                storey.nodeDescription = (StandardNodeDescription) this.nodeDescriptionStorage.ComputeIfAbsent<NodeDescription, NodeDescription>(storey.nodeDescription, <>f__am$cache0);
                storey.nodeDescription.Components.ForEach<Type>(new Action<Type>(storey.<>m__0));
                storey.nodeDescription.NotComponents.ForEach<Type>(new Action<Type>(storey.<>m__1));
                storey.nodeDescription.NotComponents.ForEach<Type>(new Action<Type>(storey.<>m__2));
                if (storey.nodeDescription.Components.Count == 0)
                {
                    this.nodeDescriptionsWithNotComponentsOnly.Add(storey.nodeDescription);
                }
                this.nodeDescriptions.Add(storey.nodeDescription);
            }
            if (Protocol != null)
            {
                foreach (Type type in storey.nodeDescription.Components)
                {
                    if (SerializationUidUtils.HasSelfUid(type))
                    {
                        Protocol.RegisterTypeWithSerialUid(type);
                    }
                }
            }
        }

        public void AssertRegister(NodeDescription nodeDescription)
        {
            if (!this.nodeDescriptions.Contains(nodeDescription))
            {
                throw new NodeNotRegisteredException(nodeDescription);
            }
        }

        public ICollection<NodeClassInstanceDescription> GetClassInstanceDescriptionByComponent(Type component)
        {
            HashSet<NodeClassInstanceDescription> set;
            return (!this.nodeClassDescsByComponent.TryGetValue(component, out set) ? ((ICollection<NodeClassInstanceDescription>) Collections.EmptyList<NodeClassInstanceDescription>()) : ((ICollection<NodeClassInstanceDescription>) set));
        }

        public ICollection<NodeDescription> GetNodeDescriptions(Type componentClass)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<ICollection<NodeDescription>>(Collections.EmptyList<NodeDescription>);
            }
            return this.nodeDescriptionsByAnyComponent.GetOrDefault<Type, ICollection<NodeDescription>>(componentClass, <>f__mg$cache0);
        }

        public ICollection<NodeDescription> GetNodeDescriptionsByNotComponent(Type componentClass)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<ICollection<NodeDescription>>(Collections.EmptyList<NodeDescription>);
            }
            return this.nodeDescriptionsByNotComponent.GetOrDefault<Type, ICollection<NodeDescription>>(componentClass, <>f__mg$cache1);
        }

        public ICollection<NodeDescription> GetNodeDescriptionsWithNotComponentsOnly() => 
            this.nodeDescriptionsWithNotComponentsOnly;

        public NodeClassInstanceDescription GetOrCreateNodeClassDescription(Type nodeClass, ICollection<Type> additionalComponents = null)
        {
            <GetOrCreateNodeClassDescription>c__AnonStorey2 storey = new <GetOrCreateNodeClassDescription>c__AnonStorey2 {
                nodeDesc = new StandardNodeDescription(nodeClass, additionalComponents)
            };
            if (storey.nodeDesc.IsEmpty)
            {
                return NodeClassInstanceDescription.EMPTY;
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = d => d;
            }
            storey.nodeDesc = (StandardNodeDescription) this.nodeDescriptionStorage.ComputeIfAbsent<NodeDescription, NodeDescription>(storey.nodeDesc, <>f__am$cache1);
            NodeClassInstanceDescription description = null;
            description = !storey.nodeDesc.isAdditionalComponents ? this.nodeClassDescByNodeClass.ComputeIfAbsent<Type, NodeClassInstanceDescription>(nodeClass, new Func<Type, NodeClassInstanceDescription>(storey.<>m__0)) : new NodeClassInstanceDescription(nodeClass, storey.nodeDesc);
            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator<Type>(description.NodeDescription.Components);
            while (enumerator.MoveNext())
            {
                this.nodeClassDescsByComponent.Add(enumerator.Current, description);
            }
            return description;
        }

        [Inject]
        public static Platform.Library.ClientProtocol.API.Protocol Protocol { get; set; }

        public ICollection<NodeDescription> NodeDescriptions
        {
            get
            {
                <>c__AnonStorey1 storey = new <>c__AnonStorey1 {
                    result = new HashSet<NodeDescription>()
                };
                this.nodeDescriptionsByAnyComponent.Values.ForEach<ICollection<NodeDescription>>(new Action<ICollection<NodeDescription>>(storey.<>m__0));
                return storey.result;
            }
        }

        [CompilerGenerated]
        private sealed class <>c__AnonStorey1
        {
            internal HashSet<NodeDescription> result;

            internal void <>m__0(ICollection<NodeDescription> x)
            {
                this.result.UnionWith(x);
            }
        }

        [CompilerGenerated]
        private sealed class <AddNodeDescription>c__AnonStorey0
        {
            internal NodeDescription nodeDescription;
            internal NodeDescriptionRegistryImpl $this;
            private static Func<Type, ICollection<NodeDescription>> <>f__am$cache0;
            private static Func<Type, ICollection<NodeDescription>> <>f__am$cache1;
            private static Func<Type, ICollection<NodeDescription>> <>f__am$cache2;

            internal void <>m__0(Type clazz)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = k => new HashSet<NodeDescription>();
                }
                this.$this.nodeDescriptionsByAnyComponent.ComputeIfAbsent<Type, ICollection<NodeDescription>>(clazz, <>f__am$cache0).Add(this.nodeDescription);
            }

            internal void <>m__1(Type clazz)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = k => new HashSet<NodeDescription>();
                }
                this.$this.nodeDescriptionsByAnyComponent.ComputeIfAbsent<Type, ICollection<NodeDescription>>(clazz, <>f__am$cache1).Add(this.nodeDescription);
            }

            internal void <>m__2(Type clazz)
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = k => new HashSet<NodeDescription>();
                }
                this.$this.nodeDescriptionsByNotComponent.ComputeIfAbsent<Type, ICollection<NodeDescription>>(clazz, <>f__am$cache2).Add(this.nodeDescription);
            }

            private static ICollection<NodeDescription> <>m__3(Type k) => 
                new HashSet<NodeDescription>();

            private static ICollection<NodeDescription> <>m__4(Type k) => 
                new HashSet<NodeDescription>();

            private static ICollection<NodeDescription> <>m__5(Type k) => 
                new HashSet<NodeDescription>();
        }

        [CompilerGenerated]
        private sealed class <GetOrCreateNodeClassDescription>c__AnonStorey2
        {
            internal StandardNodeDescription nodeDesc;

            internal NodeClassInstanceDescription <>m__0(Type k) => 
                new NodeClassInstanceDescription(k, this.nodeDesc);
        }
    }
}

