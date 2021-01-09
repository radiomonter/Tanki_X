namespace MIConvexHull
{
    using System;
    using System.Runtime.CompilerServices;

    public class TriangulationComputationConfig : ConvexHullComputationConfig
    {
        public TriangulationComputationConfig()
        {
            this.ZeroCellVolumeTolerance = 1E-05;
        }

        public double ZeroCellVolumeTolerance { get; set; }
    }
}

