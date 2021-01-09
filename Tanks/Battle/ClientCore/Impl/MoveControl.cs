namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct MoveControl
    {
        public float MoveAxis { get; set; }
        public float TurnAxis { get; set; }
        public override string ToString() => 
            $"[MoveControl MoveAxis={this.MoveAxis} TurnAxis={this.TurnAxis}]";
    }
}

