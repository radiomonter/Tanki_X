namespace MIConvexHull
{
    using System;

    internal sealed class DeferredFace
    {
        public ConvexFaceInternal Face;
        public ConvexFaceInternal Pivot;
        public ConvexFaceInternal OldFace;
        public int FaceIndex;
        public int PivotIndex;
    }
}

