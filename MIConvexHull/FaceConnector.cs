namespace MIConvexHull
{
    using System;

    internal sealed class FaceConnector
    {
        public ConvexFaceInternal Face;
        public int EdgeIndex;
        public int[] Vertices;
        public uint HashCode;
        public FaceConnector Previous;
        public FaceConnector Next;

        public FaceConnector(int dimension)
        {
            this.Vertices = new int[dimension - 1];
        }

        public static bool AreConnectable(FaceConnector a, FaceConnector b, int dim)
        {
            if (a.HashCode != b.HashCode)
            {
                return false;
            }
            int[] vertices = a.Vertices;
            int[] numArray2 = b.Vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i] != numArray2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static void Connect(FaceConnector a, FaceConnector b)
        {
            a.Face.AdjacentFaces[a.EdgeIndex] = b.Face.Index;
            b.Face.AdjacentFaces[b.EdgeIndex] = a.Face.Index;
        }

        public void Update(ConvexFaceInternal face, int edgeIndex, int dim)
        {
            int num2;
            this.Face = face;
            this.EdgeIndex = edgeIndex;
            uint num = 0x17;
            int[] vertices = face.Vertices;
            int num3 = 0;
            for (num2 = 0; num2 < edgeIndex; num2++)
            {
                this.Vertices[num3++] = vertices[num2];
                num += (uint) ((0x1f * num) + vertices[num2]);
            }
            for (num2 = edgeIndex + 1; num2 < vertices.Length; num2++)
            {
                this.Vertices[num3++] = vertices[num2];
                num += (uint) ((0x1f * num) + vertices[num2]);
            }
            this.HashCode = num;
        }
    }
}

