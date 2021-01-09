namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class StaticHit
    {
        public override string ToString() => 
            $"Position: {this.Position}, Normal: {this.Normal}";

        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }
    }
}

