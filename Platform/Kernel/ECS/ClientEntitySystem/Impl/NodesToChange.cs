namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct NodesToChange
    {
        public ICollection<NodeDescription> NodesToAdd { get; set; }
        public ICollection<NodeDescription> NodesToRemove { get; set; }
        public void Init()
        {
            this.NodesToAdd.Clear();
            this.NodesToRemove.Clear();
        }

        public NodesToChange Clone(NodesToChange original) => 
            new NodesToChange { 
                NodesToAdd = new List<NodeDescription>(original.NodesToAdd),
                NodesToRemove = new List<NodeDescription>(original.NodesToRemove)
            };
    }
}

