namespace MIConvexHull
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ConvexHullComputationConfig
    {
        public ConvexHullComputationConfig()
        {
            this.PlaneDistanceTolerance = 1E-05;
            this.PointTranslationType = MIConvexHull.PointTranslationType.None;
            this.PointTranslationGenerator = null;
        }

        private static Func<double> Closure(double radius, Random rnd)
        {
            <Closure>c__AnonStorey0 storey = new <Closure>c__AnonStorey0 {
                radius = radius,
                rnd = rnd
            };
            return new Func<double>(storey.<>m__0);
        }

        public static Func<double> RandomShiftByRadius(double radius = 1E-06, int? randomSeed = new int?())
        {
            Random rnd = (randomSeed == null) ? new Random() : new Random(randomSeed.Value);
            return Closure(radius, rnd);
        }

        public double PlaneDistanceTolerance { get; set; }

        public MIConvexHull.PointTranslationType PointTranslationType { get; set; }

        public Func<double> PointTranslationGenerator { get; set; }

        [CompilerGenerated]
        private sealed class <Closure>c__AnonStorey0
        {
            internal double radius;
            internal Random rnd;

            internal double <>m__0() => 
                this.radius * (this.rnd.NextDouble() - 0.5);
        }
    }
}

