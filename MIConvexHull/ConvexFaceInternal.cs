namespace MIConvexHull
{
    using System;

    internal sealed class ConvexFaceInternal
    {
        public int Index;
        public int[] AdjacentFaces;
        public IndexBuffer VerticesBeyond;
        public int FurthestVertex;
        public int[] Vertices;
        public double[] Normal;
        public bool IsNormalFlipped;
        public double Offset;
        public int Tag;
        public ConvexFaceInternal Previous;
        public ConvexFaceInternal Next;
        public bool InList;

        public ConvexFaceInternal(int dimension, int index, IndexBuffer beyondList)
        {
            this.Index = index;
            this.AdjacentFaces = new int[dimension];
            this.VerticesBeyond = beyondList;
            this.Normal = new double[dimension];
            this.Vertices = new int[dimension];
        }
    }
}

