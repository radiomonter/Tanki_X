namespace MIConvexHull
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class ConvexHull
    {
        [CompilerGenerated]
        private static Func<double[], DefaultVertex> <>f__am$cache0;

        public static ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> Create(IList<double[]> data, ConvexHullComputationConfig config = null)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => new DefaultVertex { Position = p.ToArray<double>() };
            }
            return ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>>.Create(data.Select<double[], DefaultVertex>(<>f__am$cache0).ToList<DefaultVertex>(), config);
        }

        public static ConvexHull<TVertex, DefaultConvexFace<TVertex>> Create<TVertex>(IList<TVertex> data, ConvexHullComputationConfig config = null) where TVertex: IVertex => 
            ConvexHull<TVertex, DefaultConvexFace<TVertex>>.Create(data, config);

        public static ConvexHull<TVertex, TFace> Create<TVertex, TFace>(IList<TVertex> data, ConvexHullComputationConfig config = null) where TVertex: IVertex where TFace: ConvexFace<TVertex, TFace>, new() => 
            ConvexHull<TVertex, TFace>.Create(data, config);
    }
}

