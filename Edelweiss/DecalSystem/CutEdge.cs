namespace Edelweiss.DecalSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct CutEdge : IComparable<CutEdge>
    {
        public int vertex1Index;
        public int vertex2Index;
        public int newVertex1Index;
        public int newVertex2Index;
        public CutEdge(int a_Vertex1Index, int a_Vertex2Index)
        {
            this.vertex1Index = a_Vertex1Index;
            this.vertex2Index = a_Vertex2Index;
            this.newVertex1Index = this.vertex1Index;
            this.newVertex2Index = this.vertex2Index;
        }

        public int SmallerIndex
        {
            get
            {
                int num = this.vertex1Index;
                if (this.vertex2Index < num)
                {
                    num = this.vertex2Index;
                }
                return num;
            }
        }
        public int GreaterIndex
        {
            get
            {
                int num = this.vertex1Index;
                if (this.vertex2Index > num)
                {
                    num = this.vertex2Index;
                }
                return num;
            }
        }
        public int ModifiedIndex =>
            (this.vertex1Index != this.newVertex1Index) ? this.newVertex1Index : this.newVertex2Index;
        public int CompareTo(CutEdge a_Other)
        {
            int num = this.SmallerIndex.CompareTo(a_Other.SmallerIndex);
            return this.GreaterIndex.CompareTo(a_Other.GreaterIndex);
        }
    }
}

