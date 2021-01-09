namespace MIConvexHull
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class ConvexFace<TVertex, TFace> where TVertex: IVertex where TFace: ConvexFace<TVertex, TFace>
    {
        protected ConvexFace()
        {
        }

        public TFace[] Adjacency { get; set; }

        public TVertex[] Vertices { get; set; }

        public double[] Normal { get; set; }
    }
}

