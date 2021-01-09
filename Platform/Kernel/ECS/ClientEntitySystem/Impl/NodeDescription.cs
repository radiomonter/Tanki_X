namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public interface NodeDescription : IComparable<NodeDescription>
    {
        ICollection<Type> BaseComponents { get; }

        ICollection<Type> Components { get; }

        ICollection<Type> NotComponents { get; }

        BitSet NodeComponentBitId { get; }

        BitSet NotNodeComponentBitId { get; }

        bool IsEmpty { get; }

        bool isAdditionalComponents { get; }
    }
}

