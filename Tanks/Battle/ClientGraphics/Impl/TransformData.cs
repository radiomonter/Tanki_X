namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct TransformData : IEquatable<TransformData>
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public bool Equals(TransformData data) => 
            (this.Position == data.Position) && (this.Rotation == data.Rotation);

        public override bool Equals(object obj) => 
            (obj is TransformData) && this.Equals((TransformData) obj);

        public override int GetHashCode() => 
            this.Position.GetHashCode() ^ this.Rotation.GetHashCode();

        public static bool operator ==(TransformData transformData1, TransformData transformData2) => 
            transformData1.Equals(transformData2);

        public static bool operator !=(TransformData transformData1, TransformData transformData2) => 
            !transformData1.Equals(transformData2);
    }
}

