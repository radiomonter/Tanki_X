namespace MIConvexHull
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ConvexHull<TVertex, TFace> where TVertex: IVertex where TFace: ConvexFace<TVertex, TFace>, new()
    {
        internal ConvexHull()
        {
        }

        public static ConvexHull<TVertex, TFace> Create(IList<TVertex> data, ConvexHullComputationConfig config)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return ConvexHullInternal.GetConvexHull<TVertex, TFace>(data, config);
        }

        public IEnumerable<TVertex> Points { get; internal set; }

        public IEnumerable<TFace> Faces { get; internal set; }
    }
}

