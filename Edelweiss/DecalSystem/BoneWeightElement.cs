namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BoneWeightElement : IComparable<BoneWeightElement>
    {
        public int index;
        public float weight;
        public int CompareTo(BoneWeightElement a_Other) => 
            -this.weight.CompareTo(a_Other.weight);
    }
}

