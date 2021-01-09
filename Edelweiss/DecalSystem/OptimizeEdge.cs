namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct OptimizeEdge : IComparable<OptimizeEdge>
    {
        public int vertex1Index;
        public int vertex2Index;
        public int triangle1Index;
        public int triangle2Index;
        public OptimizeEdge(int a_Vertex1Index, int a_Vertex2Index, int a_Triangle1Index)
        {
            if (a_Vertex1Index < a_Vertex2Index)
            {
                this.vertex1Index = a_Vertex1Index;
                this.vertex2Index = a_Vertex2Index;
            }
            else
            {
                this.vertex1Index = a_Vertex2Index;
                this.vertex2Index = a_Vertex1Index;
            }
            this.triangle1Index = a_Triangle1Index;
            this.triangle2Index = -1;
        }

        public int CompareTo(OptimizeEdge a_Other)
        {
            int num = this.vertex1Index.CompareTo(a_Other.vertex1Index);
            return this.vertex2Index.CompareTo(a_Other.vertex2Index);
        }
    }
}

