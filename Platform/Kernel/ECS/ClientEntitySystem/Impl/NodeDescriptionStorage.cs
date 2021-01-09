namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeDescriptionStorage
    {
        private readonly IDictionary<Type, ICollection<NodeDescription>> nodeDescriptionByComponentClass = new Dictionary<Type, ICollection<NodeDescription>>();
        private readonly ICollection<NodeDescription> nodes = new HashSet<NodeDescription>();

        public void AddNode(NodeDescription nodeDescription)
        {
            <AddNode>c__AnonStorey0 storey = new <AddNode>c__AnonStorey0 {
                nodeDescription = nodeDescription,
                $this = this
            };
            storey.nodeDescription.Components.ForEach<Type>(new Action<Type>(storey.<>m__0));
            this.nodes.Add(storey.nodeDescription);
        }

        public bool Contains(NodeDescription nodeDescription) => 
            this.nodes.Contains(nodeDescription);

        public virtual ICollection<NodeDescription> GetNodeDescriptions() => 
            this.nodes;

        public ICollection<NodeDescription> GetNodeDescriptions(Type componentClass)
        {
            ICollection<NodeDescription> is2;
            return (!this.nodeDescriptionByComponentClass.TryGetValue(componentClass, out is2) ? Collections.EmptyList<NodeDescription>() : is2);
        }

        public void RemoveNode(NodeDescription nodeDescription)
        {
            <RemoveNode>c__AnonStorey1 storey = new <RemoveNode>c__AnonStorey1 {
                nodeDescription = nodeDescription,
                $this = this
            };
            storey.nodeDescription.Components.ForEach<Type>(new Action<Type>(storey.<>m__0));
            this.nodes.Remove(storey.nodeDescription);
        }

        [CompilerGenerated]
        private sealed class <AddNode>c__AnonStorey0
        {
            internal NodeDescription nodeDescription;
            internal NodeDescriptionStorage $this;
            private static Func<Type, ICollection<NodeDescription>> <>f__am$cache0;

            internal void <>m__0(Type c)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = t => new HashSet<NodeDescription>();
                }
                this.$this.nodeDescriptionByComponentClass.ComputeIfAbsent<Type, ICollection<NodeDescription>>(c, <>f__am$cache0).Add(this.nodeDescription);
            }

            private static ICollection<NodeDescription> <>m__1(Type t) => 
                new HashSet<NodeDescription>();
        }

        [CompilerGenerated]
        private sealed class <RemoveNode>c__AnonStorey1
        {
            internal NodeDescription nodeDescription;
            internal NodeDescriptionStorage $this;

            internal void <>m__0(Type c)
            {
                this.$this.nodeDescriptionByComponentClass[c].Remove(this.nodeDescription);
            }
        }
    }
}

