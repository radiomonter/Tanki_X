namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct Movement
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public Quaternion Orientation { get; set; }
        public override string ToString() => 
            $"[Movement Position: {this.Position}, Velocity: {this.Velocity}, AngularVelocity: {this.AngularVelocity}, Orientation: {this.Orientation}]";
    }
}

