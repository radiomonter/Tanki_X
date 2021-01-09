namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class AbstractNodeDescription : NodeDescription, IComparable<NodeDescription>
    {
        public static readonly AbstractNodeDescription EMPTY = new AbstractNodeDescription(Collections.EmptyList<Type>());
        private readonly ICollection<Type> baseComponents;
        private readonly ICollection<Type> components;
        private readonly ICollection<Type> notComponents;
        private readonly int hashCode;
        [CompilerGenerated]
        private static Func<Type, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<Type, string> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache3;

        protected AbstractNodeDescription(ICollection<Type> components) : this(components, Collections.EmptyList<Type>(), null)
        {
        }

        protected AbstractNodeDescription(ICollection<Type> components, ICollection<Type> notComponents, ICollection<Type> additionalComponents = null)
        {
            this.baseComponents = components.ToArray<Type>();
            this.components = components;
            this.notComponents = notComponents;
            if ((additionalComponents != null) && (additionalComponents.Count > 0))
            {
                foreach (Type type in additionalComponents.Where<Type>(new Func<Type, bool>(this.<AbstractNodeDescription>m__0)))
                {
                    components.Add(type);
                }
            }
            this.NodeComponentBitId = new BitSet();
            this.NotNodeComponentBitId = new BitSet();
            this.CalcCode(components, this.NodeComponentBitId);
            this.CalcCode(notComponents, this.NotNodeComponentBitId);
            this.hashCode = this.CalcGetHashCode();
            this.IsEmpty = (components.Count == 0) && (notComponents.Count == 0);
        }

        [CompilerGenerated]
        private bool <AbstractNodeDescription>m__0(Type c) => 
            !this.baseComponents.Contains(c);

        private void CalcCode(ICollection<Type> components, BitSet componentCode)
        {
            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator<Type>(components);
            while (enumerator.MoveNext())
            {
                componentCode.Set(ComponentBitIdRegistry.GetComponentBitId(enumerator.Current));
            }
        }

        private int CalcGetHashCode() => 
            (0x1f * this.NodeComponentBitId.GetHashCode()) + this.NotNodeComponentBitId.GetHashCode();

        public int CompareTo(NodeDescription other) => 
            this.getKey().CompareTo(((AbstractNodeDescription) other).getKey());

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is AbstractNodeDescription))
            {
                return false;
            }
            AbstractNodeDescription description = (AbstractNodeDescription) obj;
            return ((this.hashCode == description.hashCode) ? (this.NodeComponentBitId.Equals(description.NodeComponentBitId) ? this.NotNodeComponentBitId.Equals(description.NotNodeComponentBitId) : false) : false);
        }

        public override int GetHashCode() => 
            this.hashCode;

        private string getKey()
        {
            // Unresolved stack state at '00000081'
        }

        public override string ToString() => 
            "AbstractNodeDescription components: " + EcsToStringUtil.ToString(this.Components) + " notComponents: " + EcsToStringUtil.ToString(this.NotComponents);

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.ComponentBitIdRegistry ComponentBitIdRegistry { get; set; }

        public bool IsEmpty { get; private set; }

        public BitSet NodeComponentBitId { get; private set; }

        public BitSet NotNodeComponentBitId { get; private set; }

        public bool isAdditionalComponents =>
            this.baseComponents.Count != this.components.Count;

        public ICollection<Type> BaseComponents =>
            this.baseComponents;

        public ICollection<Type> Components =>
            this.components;

        public ICollection<Type> NotComponents =>
            this.notComponents;
    }
}

